using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopOnlineAPI.CustomFilters;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAPICallingController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private HttpClient httpClient;

        static Stopwatch timer = Stopwatch.StartNew();
        static int count = 0;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        static object _lock1 = new object();
        static object _lock2 = new object();
        //private readonly AsyncLock _mutex = new AsyncLock();


        public ExternalAPICallingController(IConfiguration configuration)
        {
            this.configuration = configuration;
            httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetExternalAPIData()
        {
            lock (_lock1)
            {
                count++;
            }

            Console.WriteLine(count);
            Console.WriteLine(timer.Elapsed.TotalSeconds);

            if (timer.Elapsed.TotalSeconds > 15 || (timer.Elapsed.TotalSeconds <= 15 && count <= int.Parse(configuration["MaxConnectionsPerServer"].ToString())))
            {
                if (timer.Elapsed.TotalSeconds > 15)
                {
                    lock (_lock1)
                    {
                        count = 1;
                    }


                    lock (_lock2)
                    {
                        timer.Restart();
                    }
                }

                await semaphoreSlim.WaitAsync();

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync("http://localhost:57622/employee");

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    httpClient.Dispose();

                    return Ok(responseBody);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);

                    return Ok("Server is error!");
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }
            else
            {
                return Ok("External API Server is busy!");
            }

        }
    }
}
