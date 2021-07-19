using Microsoft.AspNetCore.Mvc;
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

        private Stopwatch timer = Stopwatch.StartNew();
        private int count = 0;


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            count++;
            Console.WriteLine(count);
            Console.WriteLine(timer.Elapsed.TotalSeconds);
            if (timer.Elapsed.TotalSeconds > 15 || (timer.Elapsed.TotalSeconds <= 15 && count <= int.Parse(configuration["MaxConnectionsPerServer"].ToString())))
            {
                if (timer.Elapsed.TotalSeconds > 15)
                {
                    timer.Restart();
                    count = 1;
                }

                await next();
            }
            else
            {
                string result = "<div>ExternalAPI is busy!</div>";
                byte[] bytes = Encoding.ASCII.GetBytes(result);
                await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
