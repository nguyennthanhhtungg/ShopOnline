using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopOnlineAPI.CustomFilters;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAPICallingController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly AsyncLock _mutex = new AsyncLock();


        public ExternalAPICallingController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [LimitConnections]
        public async Task<IActionResult> GetExternalAPIData()
        {
            try
            {
                using (await _mutex.LockAsync())
                {

                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.GetAsync("http://localhost:57622/employee");

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    httpClient.Dispose();

                    return Ok(responseBody);

                }

            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);

                return Ok("Server is error!");
            }
        }
    }
}
