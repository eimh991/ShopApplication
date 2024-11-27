using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class BalanceHistoryConfiguration : IEntityTypeConfiguration<BalanceHistory>
    {
        public void Configure(EntityTypeBuilder<BalanceHistory> builder)
        {
            builder.HasKey(bh=>bh.BalanceHistoryId);
            builder.Property(bh=>bh.Description)
                .HasMaxLength(250);
            builder.Property(bh=>bh.Amount)
                .IsRequired();

            builder.HasOne(bh => bh.User)
                .WithMany(u => u.BalanceHistories);
        }
    }
}
