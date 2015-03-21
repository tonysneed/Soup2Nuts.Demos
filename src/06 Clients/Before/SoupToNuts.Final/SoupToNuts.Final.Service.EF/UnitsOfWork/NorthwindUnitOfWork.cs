using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using SoupToNuts.Final.Service.EF.Contexts;
using SoupToNuts.Final.Service.Persistence.Exceptions;
using SoupToNuts.Final.Service.Persistence.Repositories;
using SoupToNuts.Final.Service.Persistence.UnitsOfWork;
using TrackableEntities.Patterns.EF6;

namespace SoupToNuts.Final.Service.EF.UnitsOfWork
{
    public class NorthwindUnitOfWork : UnitOfWork, INorthwindUnitOfWork
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public NorthwindUnitOfWork(INorthwindSlimContext context,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository) :
            base(context as DbContext)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public ICustomerRepository CustomerRepository
        {
            get { return _customerRepository; }
        }

        public IOrderRepository OrderRepository
        {
            get { return _orderRepository; }
        }

        public IProductRepository ProductRepository
        {
            get { return _productRepository; }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                throw new UpdateConcurrencyException(concurrencyException.Message,
                    concurrencyException);
            }
            catch (DbUpdateException updateException)
            {
                throw new UpdateException(updateException.Message,
                    updateException);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                throw new UpdateConcurrencyException(concurrencyException.Message,
                    concurrencyException);
            }
            catch (DbUpdateException updateException)
            {
                throw new UpdateException(updateException.Message,
                    updateException);
            }
        }
    }
}
