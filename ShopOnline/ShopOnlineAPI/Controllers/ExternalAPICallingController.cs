using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopOnlineAPI.CustomFilters;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAPICallingController : ControllerBase
    {
        //private readonly IConfiguration configuration;

        //private readonly HttpClientHandler httpClientHandler;

        //private readonly HttpClient httpClient;

        //public ExternalAPICallingController(IConfiguration configuration)
        //{
        //    this.configuration = configuration;

            //httpClientHandler = new HttpClientHandler
            //{
            //    MaxConnectionsPerServer = 1
            // };

            //httpClient = new HttpClient(httpClientHandler);
        //}

        [HttpGet]
        [LimitConnections]
        public async Task<IActionResult> GetExternalAPIData()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:57622/employee");

                //response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            //socketsHttpHandler.Dispose();
            //httpClient.Dispose();

            return Ok();
        }
    }
}
