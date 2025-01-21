using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WMS.Core.Data.Configurations.Shared;

public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter() : base(
        v => v,
        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) { }
}