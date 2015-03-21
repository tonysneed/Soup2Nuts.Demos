using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackableEntities.Patterns;
using SoupToNuts.Final.Entities.Service.Net45;

namespace SoupToNuts.Final.Service.Persistence.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>, IRepositoryAsync<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(string id);
    }
}
