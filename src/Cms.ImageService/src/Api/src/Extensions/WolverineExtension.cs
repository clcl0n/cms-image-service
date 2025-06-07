using Cms.Shared.Setups;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.RabbitMQ;

namespace Cms.ImageService.Api.Extensions;

public static class WolverineExtension
{
    public static void ConfigureWolverine(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddWolverine(options =>
        {
            options.UseRabbitMq(configuration);
        });
    }
}
