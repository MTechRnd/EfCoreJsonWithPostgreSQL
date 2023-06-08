using EFCoreJsonApp.Models.OrderWithOrderDetail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCoreJsonApp.Data
{
    public class JsonDataContext: DbContext
    {
        public DbSet<OrderWithOrderDetailEntity> OrderWithOrderDetails { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                 .AddUserSecrets<JsonDataContext>()
                 .Build();
            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderWithOrderDetailEntityConfiguration());
        }
    }
}
