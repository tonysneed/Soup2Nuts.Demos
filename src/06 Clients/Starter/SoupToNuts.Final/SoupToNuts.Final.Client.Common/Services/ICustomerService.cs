using System.Collections.Generic;
using System.Threading.Tasks;
using SoupToNuts.Final.Entities.Client.Portable.Models;

namespace SoupToNuts.Final.Client.Common.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomers();
    }
}
