using System;
using System.Data.Entity;
using SoupToNuts.Final.Entities.Service.Net45;

namespace SoupToNuts.Final.Service.EF.Contexts
{
    public interface INorthwindSlimContext
    {
        IDbSet<Category> Categories { get; set; }
        IDbSet<Customer> Customers { get; set; }
        IDbSet<CustomerSetting> CustomerSettings { get; set; }
        IDbSet<Order> Orders { get; set; }
        IDbSet<OrderDetail> OrderDetails { get; set; }
        IDbSet<Product> Products { get; set; }
        IDbSet<Employee> Employees { get; set; }
        IDbSet<Territory> Territories { get; set; }
    }
}
