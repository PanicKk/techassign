using WMS.Core.Services;
using WMS.Core.Services.IServices;
using WMS.Core.Utilities;

namespace WMS.Api.Extensions.DependencyInjection;

public static class ServicesDependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IWebhookDeliveryService, WebhookDeliveryService>();
        services.AddSingleton<IPublisherService, PublisherService>();
    }
}