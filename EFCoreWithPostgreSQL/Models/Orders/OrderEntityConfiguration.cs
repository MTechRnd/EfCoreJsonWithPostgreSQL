using EFCoreJsonApp.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreJsonApp.Models.Orders
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> modelBuilder)
        {
            modelBuilder.ToTable("Orders");

            modelBuilder.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Property(o => o.CustomerName)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            modelBuilder.Property(o => o.OrderDate)
                .HasColumnType("date");

            modelBuilder.HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Property(o => o.Timestamp)
               .IsRowVersion();

            modelBuilder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

    }
}
