using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SoupToNuts.Final.Client.Common.Services;
using SoupToNuts.Final.Entities.Client.Portable.Models;

namespace SoupToNuts.Final.WpfClient.Services
{
    public class ProductService : IProductService
    {
        public async Task<IEnumerable<Product>> GetProducts()
        {
            const string request = "api/Product";
            var response = await ServiceProxy.Instance.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<IEnumerable<Product>>(new[] { ServiceProxy.Formatter });
            return result;
        }
    }
}
