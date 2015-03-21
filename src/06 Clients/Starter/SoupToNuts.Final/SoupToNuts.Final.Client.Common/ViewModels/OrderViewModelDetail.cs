using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using SimpleMvvmToolkit;
using SoupToNuts.Final.Client.Common.Services;
using SoupToNuts.Final.Entities.Client.Portable.Models;
using TrackableEntities.Client;

namespace SoupToNuts.Final.Client.Common.ViewModels
{
    public class OrderViewModelDetail : ViewModelDetailBase<OrderViewModelDetail, Order>
    {
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;
        public event EventHandler<NotificationEventArgs<bool>> ResultNotice;

        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderViewModelDetail(IProductService productService,
            IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        // Initialization

        public void Initialize(Order order)
        {
            /*
            // Set model
            Model = order;

            // Set IsNew
            IsNew = Model.OrderId == 0;

            // Set customer name
            string customerName = Model.Customer == null
                ? Model.CustomerId
                : string.Format("{0}: {1}",
            Model.CustomerId, Model.Customer.CompanyName);
            CustomerName = customerName;

            // Set window title
            WindowTitle = IsNew ? "Add Order" : "Edit Order";

            if (!IsNew)
            {
                // Begin editing
                BeginEdit();

                // Begin change tracking
                ChangeTracker = new ChangeTrackingCollection<Order>(Model);

                // Subscribe to collection changed on order details
                Model.OrderDetails.CollectionChanged += OnOrderDetailsChanged;
            }
         */
        }

        private void OnOrderDetailsChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (eventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                // Ensure OrderId is set on new order details
                var detail = eventArgs.NewItems.Cast<OrderDetail>().FirstOrDefault();
                if (detail != null && detail.OrderId == 0)
                    detail.OrderId = Model.OrderId;
            }
        }

        private string _windowTitle;
        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                NotifyPropertyChanged(m => m.WindowTitle);
            }
        }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                NotifyPropertyChanged(m => m.CustomerName);
            }
        }

        public bool IsNew { get; private set; }

        public ChangeTrackingCollection<Order> ChangeTracker { get; private set; }

        private List<Product> _products;
        public List<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyPropertyChanged(m => m.Products);
            }
        }

        public async void LoadProducts()
        {
            try
            {
                var products = await _productService.GetProducts();
                Products = new List<Product>(products);
            }
            catch (Exception ex)
            {
                NotifyError(null, ex);
            }
        }

        // Confirm and cancel save

        /*

        public async void ConfirmSave()
        {
            if (Model == null) return;
            try
            {
                if (IsNew)
                {
                    // Save new entity
                    var createdOrder = await _orderService.CreateOrder(Model);
                    Model = createdOrder;
                }
                else
                {
                    // Get changes, exit if none
                    var changedOrder = ChangeTracker.GetChanges().SingleOrDefault();
                    if (changedOrder == null) return;

                    // Save changes
                    var updatedOrder = await _orderService.UpdateOrder(changedOrder);
                    ChangeTracker.MergeChanges(updatedOrder);

                    // Unsubscribe to collection changed on order details
                    Model.OrderDetails.CollectionChanged -= OnOrderDetailsChanged;

                    // End editing
                    EndEdit();
                }

                // Notify view of confirmation
                Notify(ResultNotice, new NotificationEventArgs<bool>(null, true));
            }
            catch (Exception ex)
            {
                NotifyError(null, ex);
            }
        }

        public void CancelSave()
        {
            // Revert changes on modified entity
            if (!IsNew)
            {
                // Unsubscribe to collection changed on order details
                Model.OrderDetails.CollectionChanged -= OnOrderDetailsChanged;

                // Cancel editing
                CancelEdit();
            }

            // Notify view of cancellation
            Notify(ResultNotice, new NotificationEventArgs<bool>(null, false));
        }
         */

        // Helper method to notify View of an error
        private void NotifyError(string message, Exception error)
        {
            // Notify view of an error
            Notify(ErrorNotice, new NotificationEventArgs<Exception>(message, error));
        }
    }
}
