using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MWS.Domain.Entities;

namespace WMS.Infrastructure.Persistens.EntityConfigurations;

public class ResourceQuantityAggregateConfiguration : IEntityTypeConfiguration<ResourceQuantityAggregate>
{
    public void Configure(EntityTypeBuilder<ResourceQuantityAggregate> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.TotalQuantity)
               .IsRequired();

        builder.HasIndex(a => new { a.ResourceId, a.UnitOfMeasureId })
               .IsUnique();
    }
}