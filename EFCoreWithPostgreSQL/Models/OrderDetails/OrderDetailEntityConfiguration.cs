using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreJsonApp.Models.OrderDetails
{
    public class OrderDetailEntityConfiguration : IEntityTypeConfiguration<OrderDetailEntity>
    {
        public void Configure(EntityTypeBuilder<OrderDetailEntity> modelBuilder)
        {
            modelBuilder.ToTable("OrderDetails");

            modelBuilder.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Property(o => o.ItemName)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            modelBuilder.Property(o => o.Price)
                .HasColumnType("double precision");

            modelBuilder.Property(o => o.Quantity)
                .HasColumnType("int");

            modelBuilder.Property(o => o.Total)
               .HasColumnType("double precision")
                .HasComputedColumnSql("\"Price\" * \"Quantity\"", stored: true);  

            modelBuilder.HasOne(o => o.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Property(o => o.Timestamp)
                .IsRowVersion();

            modelBuilder.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Property(p => p.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
