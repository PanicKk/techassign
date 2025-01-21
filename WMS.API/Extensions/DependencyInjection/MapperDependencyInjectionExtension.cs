using AutoMapper;
using AutoMapper.EquivalencyExpression;
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
            cfg.AddCollectionMappers();
        });

        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
    }
}