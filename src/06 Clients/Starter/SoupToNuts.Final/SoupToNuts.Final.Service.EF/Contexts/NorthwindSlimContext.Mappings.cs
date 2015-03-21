using System.Data.Entity;
using SoupToNuts.Final.Entities.Service.Net45;

namespace SoupToNuts.Final.Service.EF.Contexts
{
    public partial class NorthwindSlimContext
    {
        partial void ModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerSetting>()
                .HasKey(e => e.CustomerId);
        }
    }
}
