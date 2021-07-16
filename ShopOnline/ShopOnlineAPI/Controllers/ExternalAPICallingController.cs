using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAPICallingController : ControllerBase
    {
        private readonly IConfiguration configuration;

        private readonly HttpClientHandler httpClientHandler;

        private readonly HttpClient httpClient;

        public ExternalAPICallingController(IConfiguration configuration)
        {
            this.configuration = configuration;

             httpClientHandler = new HttpClientHandler
             {
                 MaxConnectionsPerServer = 1
             };

            httpClient = new HttpClient(httpClientHandler);
        }

        [HttpGet]
        public async Task<IActionResult> GetExternalAPIData()
        {
            for (int i = 0; i < 10; i++)
            {
                var employees = await httpClient.GetAsync("http://localhost:57622/employee");

                Console.WriteLine(i);
                Console.WriteLine(JsonConvert.SerializeObject(employees));
            }


            return Ok();
        }
    }
}
