using System.Collections.Generic;
using System.Threading.Tasks;
using SoupToNuts.Final.Entities.Client.Portable.Models;

namespace SoupToNuts.Final.Client.Common.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetCustomerOrders(string customerId);

        Task<Order> GetOrder(int orderId);

        Task<Order> CreateOrder(Order order);

        Task<Order> UpdateOrder(Order order);

        Task DeleteOrder(int orderId);

        Task<bool> VerifyOrderDeleted(int orderId);
    }
}
