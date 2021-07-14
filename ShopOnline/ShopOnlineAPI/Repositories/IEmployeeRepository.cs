﻿using ShopOnlineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<List<int>> GetEmployeeIdList();
    }
}
