using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(u=>u.Email)
                .IsRequired();
            builder.Property(u=>u.PasswordHash)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(e => e.Email)
            .IsUnique();

            builder.HasMany(u => u.BalanceHistories)
                .WithOne(b => b.User);
            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User);
            builder.HasOne(u => u.Cart)
                .WithOne(c => c.User);
           
        }
    }
}
