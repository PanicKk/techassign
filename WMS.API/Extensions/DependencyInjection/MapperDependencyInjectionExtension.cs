using AutoMapper;
using AutoMapper.EquivalencyExpression;
using WMS.Api.Controllers.v1;
using WMS.Core.Mappings;

namespace WMS.Api.Extensions.DependencyInjection;

public static class MapperDependencyInjectionExtension
{
    public static void AddMappingWithProfiles(this IServiceCollection services)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderMappings>();
            cfg.AddProfile<WebhookMappings>();
            cfg.AddProfile<ActivityLogMappings>();
            cfg.AddCollectionMappers();
        });

        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
    }
}