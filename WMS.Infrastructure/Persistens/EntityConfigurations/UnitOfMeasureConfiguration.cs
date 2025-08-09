using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistens.EntityConfigurations;

public class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.State)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(r => r.Title)
            .IsRequired();
    }
}