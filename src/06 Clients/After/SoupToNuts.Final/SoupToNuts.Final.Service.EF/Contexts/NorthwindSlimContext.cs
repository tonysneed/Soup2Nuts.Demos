using System.Data.Entity;
using SoupToNuts.Final.Entities.Service.Net45;

namespace SoupToNuts.Final.Service.EF.Contexts
{
    public partial class NorthwindSlimContext : DbContext, INorthwindSlimContext
    {
        static NorthwindSlimContext()
        {
            Database.SetInitializer(new NullDatabaseInitializer<NorthwindSlimContext>());
        }

        public NorthwindSlimContext()
            : base("name=NorthwindSlimContext")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<CustomerSetting> CustomerSettings { get; set; }
        public IDbSet<Employee> Employees { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderDetail> OrderDetails { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(e => e.CustomerId)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .HasOptional(e => e.CustomerSetting)
                .WithRequired(e => e.Customer);

            modelBuilder.Entity<CustomerSetting>()
                .Property(e => e.CustomerId)
                .IsFixedLength();

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Territories)
                .WithMany(e => e.Employees)
                .Map(m => m.ToTable("EmployeeTerritories").MapLeftKey("EmployeeId").MapRightKey("TerritoryId"));

            modelBuilder.Entity<Order>()
                .Property(e => e.CustomerId)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            ModelCreating(modelBuilder);
        }

        partial void ModelCreating(DbModelBuilder modelBuilder);
    }
}
