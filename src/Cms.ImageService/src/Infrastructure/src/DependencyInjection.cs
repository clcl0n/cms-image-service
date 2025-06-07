using System;
using Cms.ImageService.Infrastructure.Configurations;
using Cms.ImageService.Infrastructure.HealthChecks;
using Cms.ImageService.Infrastructure.Services;
using Cms.ImageService.Infrastructure.Services.Interfaces;
using Cms.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Cms.ImageService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        IHealthChecksBuilder healthCheckBuilder,
        IConfiguration configuration
    )
    {
        healthCheckBuilder.AddInfrastructureHealthChecks(configuration);

        services
            .AddOptions<ExternalStorageConfiguration>()
            .BindConfiguration("ExternalStorage")
            .ValidateDataAnnotations();

        services.AddSingleton<IExternalStorageService, ExternalStorageService>();
        services.AddSingleton<ILocalStorageService, LocalStorageService>();
        services.AddSingleton<IImageConvertService, ImageConvertService>();
        services.AddSingleton<IDocumentDatabaseService, DocumentDatabaseService>();
    }

    public static void AddCliInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentDatabaseService, DocumentDatabaseService>();
    }

    internal static IHealthChecksBuilder AddInfrastructureHealthChecks(
        this IHealthChecksBuilder builder,
        IConfiguration configuration
    )
    {
        builder.SetupS3HealthChecks(configuration);
        builder.AddMongoDb(
            clientFactory: _ =>
            {
                var clientSettings = MongoClientSettings.FromUrl(
                    MongoUrl.Create(configuration.GetConnectionString("MongoDb"))
                );

                return new MongoClient(clientSettings);
            },
            failureStatus: HealthStatus.Unhealthy,
            name: "mongoDb",
            tags: [HealthCheckTag.ReadinessTag, HealthCheckTag.LivenessTag],
            timeout: TimeSpan.FromSeconds(2)
        );

        return builder;
    }
}
