using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        //Get EmployeeId List in Cancelled Status by Date
        public async Task<IList<int>> GetEmployeeIdListCancelledStatusByDate(DateTime date)
        {
            var orders = await context.Set<Order>()
                .Where(o => o.UpdatedDate.Date == date.Date)
                .OrderBy(o => o.OrderId)
                .ToListAsync();

            IList<int> employeeIdList = new List<int>();

            foreach (var order in orders)
            {
                if (order.Status == "Cancelled")
                {
                    if (employeeIdList.Contains(order.EmployeeId))
                    {
                        continue;
                    }
                    else
                    {
                        employeeIdList.Add(order.EmployeeId);
                    }
                }
                else
                {
                    employeeIdList.Remove(order.EmployeeId);
                }
            }

            return employeeIdList;
        }


        //Get EmployeeId List in Completed Status by Date
        public async Task<IList<int>> GetEmployeeIdListCompletedStatusByDate(DateTime date)
        {
            var orders = await context.Set<Order>()
                .Where(o => o.UpdatedDate.Date == date.Date)
                .OrderBy(o => o.OrderId)
                .ToListAsync();

            IList<int> employeeIdList = new List<int>();


            foreach (var order in orders)
            {
                if (order.Status == "Completed")
                {
                    if (employeeIdList.Contains(order.EmployeeId))
                    {
                        continue;
                    }
                    else
                    {
                        employeeIdList.Add(order.EmployeeId);
                    }
                }
                else
                {
                    employeeIdList.Remove(order.EmployeeId);
                }
            }

            return employeeIdList;
        }


        //Get EmployeeId List in InProgress Status and Increasing Date
        public async Task<IList<int>> GetEmployeeIdListInProgressStatusAndDescendingDate()
        {
            return await context.Set<Order>()
                .Where(o => o.Status == "In Progress")
                .OrderBy(o => o.UpdatedDate)
                .Select(o => o.EmployeeId)
                .ToListAsync<int>();
        }

        //Get EmployeeId List in New Status
        public async Task<IList<int>> GetEmployeeIdListNewStatus()
        {
            return await context.Set<Order>()
                .Where(o => o.Status == "New")
                .Select(o => o.EmployeeId)
                .ToListAsync<int>();
        }
    }
}
