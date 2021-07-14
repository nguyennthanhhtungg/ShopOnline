using ShopOnlineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetWithOrderDetailListById(int id);

        //Get EmployeeId List by Date
        Task<List<int>> GetEmployeeIdListByDate(DateTime date);

        //Get EmployeeId along with number of new status List in Cancelled Status by Date
        Task<Dictionary<int, int>> GetEmployeeIdAlongNewStatusNumberListCancelledStatusByDate(DateTime date);

        //Get EmployeeId along with number of new status List in Completed Status by Date
        Task<Dictionary<int, int>> GetEmployeeIdAlongNewStatusNumberListCompletedStatusByDate(DateTime date);

        //Get EmployeeId along with number of new status List in In Progress Status by Descending Date
        Task<List<int>> GetEmployeeIdAlongNewStatusNumberListInProgressStatusAndDescendingDate();
    }
}
