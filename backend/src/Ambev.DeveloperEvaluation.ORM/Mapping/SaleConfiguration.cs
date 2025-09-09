using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Number)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();
            builder.Property(s => s.Date).IsRequired();
            builder.Property(s => s.UserId).IsRequired();
            builder.Property(s => s.Total).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(s => s.Branch).HasMaxLength(100).IsRequired();
            builder.HasMany(s => s.SaleItems)
                   .WithOne(si => si.Sale)
                   .HasForeignKey(si => si.SaleId);
        }
    }
}
