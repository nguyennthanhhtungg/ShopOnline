using Newtonsoft.Json;
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
        private readonly IEmployeeRepository employeeRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, 
            IEmployeeRepository employeeRepository)
        {
            this.orderRepository = orderRepository;
            this.orderDetailRepository = orderDetailRepository;
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

            Console.WriteLine(JsonConvert.SerializeObject(employeeIdList));

            //Get EmployeeId List in InProgress Status
            var inProgressEmployeeIdList = await orderRepository.GetEmployeeIdListInProgressStatusAndDescendingDate();

            Console.WriteLine(JsonConvert.SerializeObject(inProgressEmployeeIdList));

            //Get EmployeeId List Unavailable today
            var unavailableEmployeeIdList = await orderRepository.GetEmployeeIdListByDate(DateTime.Now);

            unavailableEmployeeIdList.AddRange(inProgressEmployeeIdList);
            unavailableEmployeeIdList = unavailableEmployeeIdList.Distinct().ToList();


            Console.WriteLine(JsonConvert.SerializeObject(unavailableEmployeeIdList));




            if (employeeIdList.Count != unavailableEmployeeIdList.Count)
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
            else
            {
                //Get EmployeeId List in Cancelled Status today
                var cancelledEmployeeIdList = await orderRepository.GetEmployeeIdListCancelledStatusByDate(DateTime.Now);

                if(cancelledEmployeeIdList.Count != 0)
                {
                    order.EmployeeId = cancelledEmployeeIdList[0];
                }
                else
                {
                    //Get EmployeeId List in Completed Status today
                    var completedEmployeeIdList = await orderRepository.GetEmployeeIdListCompletedStatusByDate(DateTime.Now);

                    if (completedEmployeeIdList.Count != 0)
                    {
                        order.EmployeeId = completedEmployeeIdList[0];
                    }
                    else
                    {
                        if(inProgressEmployeeIdList.Count == 0)
                        {
                            return order;
                        }
                        else
                        {
                            //Get EmployeeId List in New Status
                            var newEmployeeIdList = await orderRepository.GetEmployeeIdListNewStatus();

                            foreach (int id in inProgressEmployeeIdList)
                            {
                                if (newEmployeeIdList.Contains(id))
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
                    }
                }
            }

            if(order.EmployeeId == 0)
            {
                return order;
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
