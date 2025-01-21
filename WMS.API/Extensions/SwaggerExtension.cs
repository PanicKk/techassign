using System.Reflection;
using Microsoft.OpenApi.Models;

namespace WMS.Api.Extensions;

public static class SwaggerExtension
{
    public static void AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Webhook Management API",
                Version = "v1"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
        });
    }
}