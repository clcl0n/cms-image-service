using System;
using Amazon.Runtime;
using Amazon.S3;
using Cms.ImageService.Infrastructure.Configurations;

namespace Cms.ImageService.Infrastructure.Builders;

internal static class S3ConfigurationHelperBuilder
{
    public static AmazonS3Config BuildS3Config(S3ObjectStorageConfiguration configuration)
    {
        return new AmazonS3Config
        {
            ServiceURL = configuration.ServiceUrl.ToString(),
            UseHttp = configuration.ServiceUrl.Scheme == Uri.UriSchemeHttp,
            ForcePathStyle = configuration.ForcePathStyle,
        };
    }

    public static BasicAWSCredentials BuildS3Credentials(S3ObjectStorageConfiguration configuration)
    {
        return new BasicAWSCredentials(configuration.AccessKey, configuration.SecretKey);
    }
}
