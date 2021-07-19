using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineAPI.CustomFilters
{
    public class LimitConnections : Attribute, IAsyncActionFilter
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

        Stopwatch timer = Stopwatch.StartNew();
        int count = 0;


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            count++;
            Console.WriteLine(configuration["MaxConnectionsPerServer"]);
            Console.WriteLine(timer.Elapsed.TotalSeconds);
            Console.WriteLine(count);
            if (timer.Elapsed.TotalSeconds <= 5 && count <= int.Parse(configuration["MaxConnectionsPerServer"].ToString()))
            {
                await next();
            }

            if (timer.Elapsed.TotalSeconds > 5)
            {
                timer.Restart();
                count = 0;
            }


            string result = "<div>Method is busy!</div>";
            byte[] bytes = Encoding.ASCII.GetBytes(result);
            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
