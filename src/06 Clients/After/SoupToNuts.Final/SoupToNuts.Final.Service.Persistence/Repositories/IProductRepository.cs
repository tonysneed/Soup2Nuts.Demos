using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackableEntities.Patterns;
using SoupToNuts.Final.Entities.Service.Net45;

namespace SoupToNuts.Final.Service.Persistence.Repositories
{
    public interface IProductRepository : IRepository<Product>, IRepositoryAsync<Product>
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
