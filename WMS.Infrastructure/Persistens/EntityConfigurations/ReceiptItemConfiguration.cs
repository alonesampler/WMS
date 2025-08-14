using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistens.EntityConfigurations;

public class ReceiptItemConfiguration : IEntityTypeConfiguration<ReceiptItem>
{
    public void Configure(EntityTypeBuilder<ReceiptItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Quantity)
               .IsRequired();

        builder.HasOne(i => i.ReceiptDocument)
               .WithMany(d => d.Items)
               .HasForeignKey(i => i.ReceiptDocumentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Resource)
               .WithMany()
               .HasForeignKey(i => i.ResourceId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.UnitOfMeasure)
               .WithMany()
               .HasForeignKey(i => i.UnitOfMeasureId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}