using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TrackableEntities.Patterns.EF6;
using SoupToNuts.Final.Entities.Service.Net45;
using SoupToNuts.Final.Service.EF.Contexts;
using SoupToNuts.Final.Service.Persistence.Repositories;

namespace SoupToNuts.Final.Service.EF.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly INorthwindSlimContext _context;

        public OrderRepository(INorthwindSlimContext context) :
            base(context as DbContext)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            IEnumerable<Order> entities = await _context.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                .ToListAsync();
            return entities;
        }

        public async Task<Order> GetOrder(int id)
        {
            Order entity = await _context.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                 .SingleOrDefaultAsync(t => t.OrderId == id);
            return entity;
        }

        public async Task<IEnumerable<Order>> GetOrders(string customerId)
        {
            IEnumerable<Order> orders = await _context.Orders
                .Include(o => o.Customer)
                .Include("OrderDetails.Product")
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
            return orders;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            Order entity = await _context.Orders
                .Include(o => o.OrderDetails) // Include details
                .SingleOrDefaultAsync(t =>   t.OrderId == id);
            if (entity == null) return false;
            ApplyDelete(entity);
            return true;
        }
    }
}
