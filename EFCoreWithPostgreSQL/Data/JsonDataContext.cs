using EFCoreJsonApp.Comman;
using EFCoreJsonApp.Models.OrderDetails;
using EFCoreJsonApp.Models.OrderWithOrderDetail;
using EFCoreJsonApp.Models.Records;
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
            modelBuilder.Entity<TotalPriceResult>().ToTable("TotalPriceResult", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.TotalPrice).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<TotalQuantityResult>().ToTable("TotalQuantityResult", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.TotalQuantity).HasColumnType("int");
            modelBuilder.Entity<AverageOfPriceResult>().ToTable("AverageOfPriceResult", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.AverageOfPrice).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<AverageOfQuantityResult>().ToTable("AverageOfQuantityResult", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.AverageOfQuantity).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<MaxQuantityResult>().ToTable("MaxQuantity", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.MaximumQuantity).HasColumnType("int");
            modelBuilder.Entity<MinQuantityResult>().ToTable("MinQuantity", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.MinimumQuantity).HasColumnType("int");
            modelBuilder.Entity<TotalByOrderResult>().ToTable("TotalByOrder", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.TotalByOrderId).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<MaxPriceResult>().ToTable("MaxPrice", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.MaximumPrice).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<MinPriceResult>().ToTable("MinPrice", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.MinimumPrice).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<TotalOrderByCustomerResult>().ToTable("TotalOrderByCustomerResult", t => t.ExcludeFromMigrations()).HasNoKey().Property(p => p.TotalOrderByCustomerId).HasColumnType("int"); ;
            modelBuilder.Entity<OrderCount>().ToTable("OrderCount", t => t.ExcludeFromMigrations()).HasNoKey();
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Modified || e.State == EntityState.Added));
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((BaseEntity)entity.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
