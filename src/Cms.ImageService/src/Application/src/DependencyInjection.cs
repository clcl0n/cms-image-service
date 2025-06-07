using Cms.ImageService.Application.Services.Interfaces;
using Cms.ImageService.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cms.ImageService.Application;

public static class DependencyInjection
{
    public static void AddApplication(
        this IServiceCollection services,
        IHealthChecksBuilder healthCheckBuilder,
        IConfiguration configuration
    )
    {
        services.AddInfrastructure(healthCheckBuilder, configuration);

        services.AddScoped<IImageService, Services.ImageService>();
    }
}
