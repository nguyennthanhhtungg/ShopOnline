using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShopOnlineDBContext context) : base(context)
        {
        }

        public async Task<Order> GetWithOrderDetailListById(int id)
        {
            return await context.Set<Order>().Where(o => o.OrderId == id).Include(o => o.OrderDetails).FirstOrDefaultAsync();
        }


        //Get EmployeeId List by Date
        public async Task<List<int>> GetEmployeeIdListByDate(DateTime date)
        {
            return await context.Set<Order>()
                .Where(o => o.UpdatedDate.Date == date.Date)
                .Select(o => o.EmployeeId)
                .Distinct()
                .ToListAsync<int>();
        }

        //Get EmployeeId along with number of new status List in Cancelled Status by Date
        public async Task<Dictionary<int, int>> GetEmployeeIdAlongNewStatusNumberListCancelledStatusByDate(DateTime date)
        {
            var orders = await context.Set<Order>()
                .Where(o => o.UpdatedDate.Date == date.Date)
                .ToListAsync();

            Dictionary<int, int> employeeIdAlongNewStatusNumberList = new Dictionary<int, int>();

            foreach(var order in orders)
            {
                if(order.Status == "Cancelled")
                {
                    if (employeeIdAlongNewStatusNumberList.ContainsKey(order.EmployeeId))
                    {
                        continue;
                    }
                    else
                    {
                        employeeIdAlongNewStatusNumberList.Add(order.EmployeeId, 0);
                    }
                }
                else if(order.Status == "New")
                {
                    if (employeeIdAlongNewStatusNumberList.ContainsKey(order.EmployeeId))
                    {
                        employeeIdAlongNewStatusNumberList[order.EmployeeId] += 1;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    employeeIdAlongNewStatusNumberList.Remove(order.EmployeeId);
                }
            }

            return employeeIdAlongNewStatusNumberList.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }


        //Get EmployeeId along with number of new status List in Completed Status by Date
        public async Task<Dictionary<int, int>> GetEmployeeIdAlongNewStatusNumberListCompletedStatusByDate(DateTime date)
        {
            var orders = await context.Set<Order>()
                .Where(o => o.UpdatedDate.Date == date.Date)
                .ToListAsync();

            Dictionary<int, int> employeeIdAlongNewStatusNumberList = new Dictionary<int, int>();


            foreach (var order in orders)
            {
                if (order.Status == "Completed")
                {
                    if (employeeIdAlongNewStatusNumberList.ContainsKey(order.EmployeeId))
                    {
                        continue;
                    }
                    else
                    {
                        employeeIdAlongNewStatusNumberList.Add(order.EmployeeId, 0);
                    }
                }
                else if (order.Status == "New")
                {
                    if (employeeIdAlongNewStatusNumberList.ContainsKey(order.EmployeeId))
                    {
                        employeeIdAlongNewStatusNumberList[order.EmployeeId] += 1;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    employeeIdAlongNewStatusNumberList.Remove(order.EmployeeId);
                }
            }

            return employeeIdAlongNewStatusNumberList.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }


        //Get EmployeeId List in In Progress Status
        public async Task<List<int>> GetEmployeeIdAlongNewStatusNumberListInProgressStatusAndDescendingDate()
        {
            return await context.Set<Order>()
                .Where(o => o.Status == "In Progress")
                .OrderByDescending(o => o.UpdatedDate)
                .Select(o => o.EmployeeId)
                .ToListAsync<int>();
        }
    }
}
