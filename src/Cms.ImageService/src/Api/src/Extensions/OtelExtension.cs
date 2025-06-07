using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Cms.ImageService.Api.Extensions;

public static class OtelExtension
{
    public static void ConfigureOtel(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
                metrics
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddOperatingSystemDetector()
                    )
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter()
            )
            .WithTracing(tracing =>
                tracing
                    .SetResourceBuilder(
                        ResourceBuilder
                            .CreateDefault()
                            .AddContainerDetector()
                            .AddOperatingSystemDetector()
                    )
                    .AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources")
                    .AddSource("Wolverine")
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter()
            );
    }

    public static void ConfigureOtel(this ILoggingBuilder logging)
    {
        logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;

            options
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddOperatingSystemDetector())
                .AddOtlpExporter();
        });
    }
}
