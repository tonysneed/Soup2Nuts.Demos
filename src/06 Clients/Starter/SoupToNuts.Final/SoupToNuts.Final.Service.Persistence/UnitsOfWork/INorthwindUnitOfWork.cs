using System;
using SoupToNuts.Final.Service.Persistence.Repositories;
using TrackableEntities.Patterns;

namespace SoupToNuts.Final.Service.Persistence.UnitsOfWork
{
    public interface INorthwindUnitOfWork : IUnitOfWork, IUnitOfWorkAsync
    {
        ICustomerRepository CustomerRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductRepository ProductRepository { get; }
    }
}
