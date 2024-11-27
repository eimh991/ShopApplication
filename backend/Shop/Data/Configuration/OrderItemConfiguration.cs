using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.OrderItemId);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems);
            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems);
        }
    }
}
