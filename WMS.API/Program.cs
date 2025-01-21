using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WMS.Api.Extensions;
using WMS.Api.Extensions.DependencyInjection;
using WMS.Api.Middlewares;
using WMS.Core.Data;
using WMS.Core.Repositories;
using WMS.MessageBroker.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

services.AddEndpointsApiExplorer();
services.AddMediatR();
services.AddMappingWithProfiles();
services.AddConfigurations(builder.Configuration);

services.RegisterConsumers();

services.AddCustomSwaggerGen();

services.AddScoped(typeof(IRepository<>), typeof(WMSRepository<>));

services.AddDbContext<WMSDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["Database:ConnectionString"],
            npgsqlOptionsAction: npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__MigrationsHistory");

                npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);

                npgsqlOptions.MigrationsAssembly("WMS.Migrations");
            })
        .UseSnakeCaseNamingConvention()
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});

services.RegisterServices();

services.AddHttpClient();
services.AddHttpContextAccessor();

services.AddCors(options =>
{
    // var origins = builder.Configuration.GetSection("CorsOrigins").GetSection("AllowedOrigins").Get<string[]>();
    //
    // var exposedHeaders = builder.Configuration.GetSection("CorsOrigins")
    //                             .GetSection("ExposedHeaders")
    //                             .Get<string[]>();

    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.DisplayRequestDuration(); });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseConsumers();

app.MapControllers();

app.Seed();
app.Run();