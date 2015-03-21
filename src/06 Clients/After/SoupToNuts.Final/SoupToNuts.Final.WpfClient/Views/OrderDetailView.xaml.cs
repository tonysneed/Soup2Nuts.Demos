using System;
using System.Windows;
using SimpleMvvmToolkit;
using SoupToNuts.Final.Client.Common.ViewModels;
using SoupToNuts.Final.Entities.Client.Portable.Models;

namespace SoupToNuts.Final.WpfClient.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailView.xaml
    /// </summary>
    public partial class OrderDetailView : Window
    {
        public OrderViewModelDetail ViewModel { get; private set; }

        public OrderDetailView(Order order)
        {
            InitializeComponent();

            // Initialize view model
            ViewModel = (OrderViewModelDetail)DataContext;
            ViewModel.Initialize(order);
            ViewModel.ErrorNotice += OnErrorNotice;
            ViewModel.ResultNotice += OnResultNotice;
        }

        private void OnResultNotice(object sender, NotificationEventArgs<bool> eventArgs)
        {
            DialogResult = eventArgs.Data;
        }

        private void OnErrorNotice(object sender, NotificationEventArgs<Exception> eventArgs)
        {
            MessageBox.Show(eventArgs.Data.Message, "Error");
        }

        private void OrderDetailView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ErrorNotice -= OnErrorNotice;
            ViewModel.ResultNotice -= OnResultNotice;
        }
    }
}
