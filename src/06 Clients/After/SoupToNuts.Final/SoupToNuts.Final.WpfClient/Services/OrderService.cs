﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SoupToNuts.Final.Client.Common.Services;
using SoupToNuts.Final.Entities.Client.Portable.Models;

namespace SoupToNuts.Final.WpfClient.Services
{
    public class OrderService : IOrderService
    {
        public async Task<IEnumerable<Order>> GetCustomerOrders(string customerId)
        {
            string request = "api/Order?customerId=" + customerId;
            var response = await ServiceProxy.Instance.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<IEnumerable<Order>>(new[] { ServiceProxy.Formatter });
            return result;
        }

        public async Task<Order> GetOrder(int orderId)
        {
            string request = "api/Order/" + orderId;
            var response = await ServiceProxy.Instance.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<Order>(new[] { ServiceProxy.Formatter });
            return result;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            string request = "api/Order";
            var response = await ServiceProxy.Instance.PostAsync(request, order, ServiceProxy.Formatter);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<Order>(new[] { ServiceProxy.Formatter });
            return result;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            string request = "api/Order";
            var response = await ServiceProxy.Instance.PutAsync(request, order, ServiceProxy.Formatter);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<Order>(new[] { ServiceProxy.Formatter });
            return result;
        }

        public async Task DeleteOrder(int orderId)
        {
            string request = "api/Order/" + orderId;
            var response = await ServiceProxy.Instance.DeleteAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> VerifyOrderDeleted(int orderId)
        {
            string request = "api/Order/" + orderId;
            var response = await ServiceProxy.Instance.GetAsync(request);
            if (response.IsSuccessStatusCode) return false;
            return true;
        }
    }
}
