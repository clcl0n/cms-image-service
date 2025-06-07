using System.Text.Json.Serialization;
using Cms.ImageService.Api.Extensions;
using Cms.ImageService.Application;
using Cms.Shared.Setups;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cms.ImageService.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ConfigureOtel();

        builder.Services.ConfigureOtel();

        var healthCheckBuilder = builder.Services.AddHealthChecks();

        builder.Services.AddApplication(healthCheckBuilder, builder.Configuration);

        builder
            .Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    // show enum value in swagger.
                    new JsonStringEnumConverter()
                );
            });

        builder.Services.SetupApiConfiguration(builder.Configuration);

        healthCheckBuilder.ConfigureHealthCheck(builder.Configuration);

        builder.Services.ConfigureWolverine(builder.Configuration);

        using var app = builder.Build();

        // app.UseExceptionHandler();
        app.UseStatusCodePages();

        app.UseHealthCheck();

        app.MapControllers();

        app.Run();
    }
}
