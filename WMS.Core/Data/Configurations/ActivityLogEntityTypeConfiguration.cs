using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Core.Data.Configurations.Shared;
using WMS.Models.Entities;

namespace WMS.Core.Data.Configurations;

public class ActivityLogEntityTypeConfiguration : BaseEntityTypeConfiguration<ActivityLog>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.HasKey(activityLog => activityLog.Id);

        foreach (var property in builder.Metadata.GetProperties())
        {
            if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
            {
                property.SetValueConverter(new UtcDateTimeConverter());
            }
        }
    }
}