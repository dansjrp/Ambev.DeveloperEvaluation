using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.HasKey(si => si.Id);
            builder.Property(si => si.SaleId).IsRequired();
            builder.Property(si => si.ProductId).IsRequired();
            builder.Property(si => si.Quantity).IsRequired();
            builder.Property(si => si.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(si => si.Discounts).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(si => si.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(si => si.Cancelled).IsRequired();
        }
    }
}
