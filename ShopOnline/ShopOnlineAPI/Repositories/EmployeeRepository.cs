using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ShopOnlineDBContext context) : base(context)
        {
        }

        //Get EmployeeId available for Order
        public async Task<List<int>> GetEmployeeIdList()
        {
            return await context.Set<Employee>()
                .Select(emp => emp.EmployeeId).ToListAsync();
        }
    }
}
