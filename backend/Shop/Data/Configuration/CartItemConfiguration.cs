using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci=>ci.CartItemId);

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems);
            builder.HasOne(ci=>ci.Product)
                .WithMany(p=>p.CartItems);
        }
    }
}
