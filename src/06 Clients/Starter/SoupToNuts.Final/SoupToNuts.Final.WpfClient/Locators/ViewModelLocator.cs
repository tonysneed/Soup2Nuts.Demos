using System;
using SimpleInjector;
using SoupToNuts.Final.Client.Common.Services;
using SoupToNuts.Final.Client.Common.ViewModels;
using SoupToNuts.Final.WpfClient.Services;

namespace SoupToNuts.Final.WpfClient.Locators
{
    public class ViewModelLocator
    {
        private readonly Container _container;

        public ViewModelLocator()
        {
            // Set up DI container
            _container = new Container();
            Bootstrap();
        }

        private void Bootstrap()
        {
            // Register services
            //_container.Register<IProductService, ProductService>(Lifestyle.Transient);
            //_container.Register<ICustomerService, CustomerService>(Lifestyle.Transient);
            //_container.Register<IOrderService, OrderService>(Lifestyle.Transient);

            // Register view models
            //_container.Register<MainPageViewModel>(Lifestyle.Transient);
            //_container.Register<CustomerOrdersViewModel>(Lifestyle.Transient);
            //_container.Register<OrderViewModelDetail>(Lifestyle.Transient);
        }

        public MainPageViewModel MainPageViewModel
        {
            get { return _container.GetInstance<MainPageViewModel>(); }
        }

        public CustomerOrdersViewModel CustomerOrdersViewModel
        {
            get { return _container.GetInstance<CustomerOrdersViewModel>(); }
        }

        public OrderViewModelDetail OrderViewModelDetail
        {
            get { return _container.GetInstance<OrderViewModelDetail>(); }
        }
    }
}
