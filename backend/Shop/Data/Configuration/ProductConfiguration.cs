using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        [Obsolete]
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p=>p.ProductId);
            builder.Property(p => p.Name)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(p => p.Price)
                .HasDefaultValue(0.01m)
                .IsRequired();
            builder.Property(p=>p.Description)
                .HasMaxLength(500);

            //builder.HasCheckConstraint("CK_Product_Price", "Price > 0"); -  попытался сделать  проверку что цена меньше 0 (пока что не вышло)

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products);
            builder.HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product);
            builder.HasMany(p => p.CartItems)
                .WithOne(c => c.Product);
        }
    }
}
