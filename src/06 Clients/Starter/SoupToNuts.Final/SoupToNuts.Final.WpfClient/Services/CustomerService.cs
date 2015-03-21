using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SoupToNuts.Final.Client.Common.Services;
using SoupToNuts.Final.Entities.Client.Portable.Models;

namespace SoupToNuts.Final.WpfClient.Services
{
    public class CustomerService : ICustomerService
    {
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            const string request = "api/Customer";
            var response = await ServiceProxy.Instance.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<IEnumerable<Customer>>(new[] { ServiceProxy.Formatter });
            return result;
        }
    }
}
