using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreJsonApp.Models.OrderWithOrderDetail
{
    public class OrderWithOrderDetailEntityConfiguration : IEntityTypeConfiguration<OrderWithOrderDetailEntity>
    {
        public void Configure(EntityTypeBuilder<OrderWithOrderDetailEntity> modelBuilder)
        {
            modelBuilder.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Property(o => o.CustomerName)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            modelBuilder.Property(o => o.OrderDate)
                .HasColumnType("date");

            modelBuilder.Property(o => o.OrderDetailsJson)
                .HasColumnType("jsonb");

            modelBuilder.Property(p => p.Timestamp).IsRowVersion();

            modelBuilder.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Property(p => p.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
