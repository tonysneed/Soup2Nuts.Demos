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
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly INorthwindSlimContext _context;

        public CustomerRepository(INorthwindSlimContext context) :
            base(context as DbContext)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            IEnumerable<Customer> entities = await _context.Customers
                .ToListAsync();
            return entities;
        }

        public async Task<Customer> GetCustomer(string id)
        {
            Customer entity = await _context.Customers
                 .SingleOrDefaultAsync(t => t.CustomerId == id);
            return entity;
        }
    }
}
