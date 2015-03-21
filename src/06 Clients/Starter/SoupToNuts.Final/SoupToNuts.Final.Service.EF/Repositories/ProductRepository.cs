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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly INorthwindSlimContext _context;

        public ProductRepository(INorthwindSlimContext context) :
            base(context as DbContext)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            IEnumerable<Product> entities = await _context.Products
                .ToListAsync();
            return entities;
        }
    }
}
