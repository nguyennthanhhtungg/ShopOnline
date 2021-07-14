using ShopOnlineAPI.Models;
using ShopOnlineAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly IProductRepository productRepository;
        private readonly IEmployeeRepository employeeRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, 
            IProductRepository productRepository, IEmployeeRepository employeeRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.productRepository = productRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task<Order> Add(Order order)
        {
            await orderRepository.Add(order);
            return order;
        }

        public async Task<Order> Add(Order order, List<OrderDetail> orderDetailList)
        {
            //Get EmployeeId List
            var employeeIdList = await employeeRepository.GetEmployeeIdList();

            //Get EmployeeId List unavailable today
            var unavailableEmployeeIdList = await orderRepository.GetEmployeeIdListByDate(DateTime.Now);

            if (unavailableEmployeeIdList.Count != 0)
            {
                foreach (int id in employeeIdList)
                {
                    if (unavailableEmployeeIdList.Contains(id))
                    {
                        continue;
                    }
                    else
                    {
                        order.EmployeeId = id;
                        break;
                    }
                }
            }

            if (order.EmployeeId == 0)
            {
                //Get EmployeeId along with number of new status List with Cancelled Status today
                var cancelledEmployeeIdList = await orderRepository.GetEmployeeIdAlongNewStatusNumberListCancelledStatusByDate(DateTime.Now);

                if(cancelledEmployeeIdList.Count != 0 && cancelledEmployeeIdList.First().Value == 0)
                {
                    order.EmployeeId = cancelledEmployeeIdList.First().Key;
                }


                if(order.EmployeeId == 0)
                {
                    //Get EmployeeId along with number of new status List with Complted Status today
                    var completedEmployeeIdList = await orderRepository.GetEmployeeIdAlongNewStatusNumberListCompletedStatusByDate(DateTime.Now);

                    if (completedEmployeeIdList.Count != 0 && completedEmployeeIdList.First().Value == 0)
                    {
                        order.EmployeeId = completedEmployeeIdList.First().Value;
                    }

                    if (order.EmployeeId == 0)
                    {
                        //Get EmployeeId along with number of new status List with In Progress Status and Descending Date
                        var inProgressEmployeeIdList = await orderRepository.GetEmployeeIdAlongNewStatusNumberListInProgressStatusAndDescendingDate();

                        if (inProgressEmployeeIdList.Count != 0)
                        {
                            order.EmployeeId = inProgressEmployeeIdList.First();
                        }

                        if (order.EmployeeId == 0 && cancelledEmployeeIdList.Count != 0)
                        {
                            order.EmployeeId = cancelledEmployeeIdList.First().Key;
                        }

                        if (order.EmployeeId == 0 && completedEmployeeIdList.Count != 0)
                        {
                            order.EmployeeId = completedEmployeeIdList.First().Key;
                        }
                    }

                    if (order.EmployeeId == 0)
                    {
                        order.EmployeeId = employeeIdList.First();
                    }
                }
            }


            await orderRepository.Add(order);

            if (order.OrderId != 0)
            {
                foreach (var orderDetail in orderDetailList)
                {
                    orderDetail.OrderId = order.OrderId;
                }

                await orderDetailRepository.AddRange(orderDetailList);

                order.OrderDetails = orderDetailList;
            }

            return order;
        }

        public async Task<List<Order>> GetAll()
        {
            var orders = await orderRepository.GetAll();
            return orders.ToList();
        }

        public async Task<Order> GetById(int id)
        {
            var order = await orderRepository.GetById(id);
            return order;
        }

        public async Task<Order> GetWithOrderDetailListById(int id)
        {
            var order = await orderRepository.GetWithOrderDetailListById(id);
            return order;
        }

        public async Task<Order> Update(Order order)
        {
            await orderRepository.Update(order);
            return order;
        }
    }
}
