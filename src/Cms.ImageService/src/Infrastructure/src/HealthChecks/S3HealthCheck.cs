using System;
using Cms.ImageService.Infrastructure.Builders;
using Cms.ImageService.Infrastructure.Configurations;
using Cms.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Cms.ImageService.Infrastructure.HealthChecks;

internal static class S3HealthCheck
{
    internal static IHealthChecksBuilder SetupS3HealthChecks(
        this IHealthChecksBuilder builder,
        IConfiguration configuration
    )
    {
        builder.AddS3(
            setup: options =>
            {
                var s3Configuration =
                    configuration
                        .GetSection("ExternalStorage")
                        .Get<ExternalStorageConfiguration>()
                        ?.S3ObjectStorage
                    ?? throw new InvalidOperationException(
                        "ExternalStorage configuration is missing."
                    );

                options.S3Config = S3ConfigurationHelperBuilder.BuildS3Config(s3Configuration);
                options.BucketName = s3Configuration.ImageBucketName;
                options.Credentials = S3ConfigurationHelperBuilder.BuildS3Credentials(
                    s3Configuration
                );
            },
            failureStatus: HealthStatus.Unhealthy,
            name: "S3ImageBucket",
            tags: [HealthCheckTag.ReadinessTag, HealthCheckTag.LivenessTag],
            timeout: TimeSpan.FromSeconds(2)
        );

        return builder;
    }
}
