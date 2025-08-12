using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistens.EntityConfigurations;

public class ReceiptDocumentConfiguration : IEntityTypeConfiguration<ReceiptDocument>
{
    public void Configure(EntityTypeBuilder<ReceiptDocument> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApplicationNumber)
            .IsRequired();
        
        builder.Property(x => x.Date)
            .IsRequired();
    }
}