using WMS.Core.Configuration;

namespace WMS.Api.Extensions.DependencyInjection;

public static class MediatRDependencyInjectionExtensions
{
    public static void AddMediatR(this IServiceCollection services)
    {
        var assemblyApp = typeof(Program).Assembly;
        var assemblyCore = typeof(DatabaseConfiguration).Assembly;

        services.AddMediatR(config => { config.RegisterServicesFromAssemblies(assemblyApp, assemblyCore); });
    }
}