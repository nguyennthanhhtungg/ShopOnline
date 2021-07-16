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

        //Get EmployeeId List in Cancelled Status by Date
        Task<IList<int>> GetEmployeeIdListCancelledStatusByDate(DateTime date);

        //Get EmployeeId List in Completed Status by Date
        Task<IList<int>> GetEmployeeIdListCompletedStatusByDate(DateTime date);

        //Get EmployeeId List in InProgress Status and Increasing Date
        Task<IList<int>> GetEmployeeIdListInProgressStatusAndDescendingDate();

        //Get EmployeeId List in New Status
        Task<IList<int>> GetEmployeeIdListNewStatus();
    }
}
