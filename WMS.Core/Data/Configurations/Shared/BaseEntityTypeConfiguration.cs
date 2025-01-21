using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Models.Entities.Shared;

namespace WMS.Core.Data.Configurations.Shared;

public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedAt)
               .HasDefaultValueSql("timezone('UTC', now())")
               .ValueGeneratedOnAdd()
               .IsRequired();

        builder.Property(x => x.UpdatedAt)
               .HasDefaultValueSql("timezone('UTC', now())")
               .ValueGeneratedOnUpdate();

        builder.Property(x => x.IsDeleted)
               .HasDefaultValue(false)
               .ValueGeneratedOnAdd()
               .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);

        foreach (var property in builder.Metadata.GetProperties())
        {
            if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
            {
                property.SetValueConverter(new UtcDateTimeConverter());
            }
        }

        ConfigureEntity(builder);
    }

    protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}