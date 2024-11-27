using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;


namespace Shop.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderId);
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders);
            builder.HasMany(o => o.OrderItems)
                .WithOne(orderI => orderI.Order);
        }
    }
}
