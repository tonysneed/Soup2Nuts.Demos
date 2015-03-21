using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SoupToNuts.Final.WebApi;
using SoupToNuts.Final.Service.EF.Contexts;
using SoupToNuts.Final.Service.EF.Repositories;
using SoupToNuts.Final.Service.EF.UnitsOfWork;
using SoupToNuts.Final.Service.Persistence.Repositories;
using SoupToNuts.Final.Service.Persistence.UnitsOfWork;

[assembly: WebActivator.PostApplicationStartMethod(typeof(SimpleInjectorWebApiInitializer), "Initialize")]

namespace SoupToNuts.Final.WebApi
{
    public static class SimpleInjectorWebApiInitializer
    {
        public static void Initialize()
        {
            // Create IoC container
            var container = new Container();

            // Register dependencies
            InitializeContainer(container);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            // Verify registrations
            container.Verify();

            // Set Web API dependency resolver
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }

        private static void InitializeContainer(Container container)
        {
            // Register context, unit of work and repos with per request lifetime
            container.RegisterWebApiRequest<INorthwindSlimContext, NorthwindSlimContext>();
            container.RegisterWebApiRequest<INorthwindUnitOfWork, NorthwindUnitOfWork>();
            container.RegisterWebApiRequest<ICustomerRepository, CustomerRepository>();
            container.RegisterWebApiRequest<IOrderRepository, OrderRepository>();
            container.RegisterWebApiRequest<IProductRepository, ProductRepository>();
        }
    }
}