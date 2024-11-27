using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c=>c.CartId);

            builder.HasOne(c => c.User)
                .WithOne(u => u.Cart);
            builder.HasMany(c=>c.CartItems)
                .WithOne(ci=>ci.Cart);
        }
    }
}
