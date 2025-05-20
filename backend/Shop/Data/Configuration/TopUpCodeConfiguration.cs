using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shop.Model;

namespace Shop.Data.Configuration
{
    public class TopUpCodeConfiguration : IEntityTypeConfiguration<TopUpCode>
    {
        public void Configure(EntityTypeBuilder<TopUpCode> builder)
        {
            builder.HasKey(c => c.TopUpCodeId);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.Amount)
                   .IsRequired()
                   .HasConversion<int>(); 

            builder.Property(c => c.IsUsed)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(c => c.UsedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.UsedByUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
