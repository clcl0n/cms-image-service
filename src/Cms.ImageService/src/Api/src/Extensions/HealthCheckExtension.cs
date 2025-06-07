using Cms.Shared.Constants;
using Cms.Shared.Setups;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cms.ImageService.Api.Extensions;

public static class HealthCheckExtension
{
    public static void ConfigureHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration
    )
    {
        builder
            .AddRabbitMQ(configuration)
            .AddApplicationLifecycleHealthCheck([HealthCheckTag.StartupTag]);
    }
}
