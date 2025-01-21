using WMS.Core.Data;

namespace WMS.Api.Extensions;

public static class IHostExtension
{
    public static IHost Seed(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<WMSDbContext>();

        var dataInitializer = new DataInitializer(dbContext);

        dataInitializer.EnsureMigrated();

        return host;
    }
}