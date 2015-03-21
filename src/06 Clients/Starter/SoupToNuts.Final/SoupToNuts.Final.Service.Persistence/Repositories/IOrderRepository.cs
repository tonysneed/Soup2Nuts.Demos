using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackableEntities.Patterns;
using SoupToNuts.Final.Entities.Service.Net45;

namespace SoupToNuts.Final.Service.Persistence.Repositories
{
    public interface IOrderRepository : IRepository<Order>, IRepositoryAsync<Order>
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<IEnumerable<Order>> GetOrders(string customerId);
        Task<Order> GetOrder(int id);
        Task<bool> DeleteOrder(int id);
    }
}
