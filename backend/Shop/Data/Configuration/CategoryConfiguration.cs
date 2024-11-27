using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c=>c.CategoryId);
            builder.Property(c => c.CategoryName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
