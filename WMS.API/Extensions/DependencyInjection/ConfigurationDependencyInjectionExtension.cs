using WMS.Core.Configuration;
using WMS.MessageBroker.Configuration;

namespace WMS.Api.Extensions.DependencyInjection;

public static class ConfigurationDependencyInjectionExtension
{
    public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        BindDatabaseConfiguration(services, configuration);
        BindRabbitMQConfiguration(services, configuration);
    }

    private static void BindDatabaseConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfiguration = new DatabaseConfiguration();
        configuration.Bind("Database", databaseConfiguration);
        services.AddSingleton(databaseConfiguration);
    }

    private static void BindRabbitMQConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfiguration = new RabbitMqConfiguration();
        configuration.Bind("RabbitMQ", rabbitMqConfiguration);
        services.AddSingleton(rabbitMqConfiguration);
    }
}