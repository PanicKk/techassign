using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WMS.MessageBroker.Abstraction;

namespace WMS.MessageBroker.Extensions.DependencyInjection;

public static class ConsumerDependencyInjectionExtensions
{
    public static void RegisterConsumers(this IServiceCollection services)
    {
        foreach (var consumer in GetAllConsumers())
            services.AddSingleton(consumer);
    }

    public static void UseConsumers(this IApplicationBuilder app)
    {
        foreach (var consumer in GetAllConsumers())
            app.ApplicationServices.GetRequiredService(consumer);
    }

    private static List<Type> GetAllConsumers()
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(IConsumer).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
    }
}