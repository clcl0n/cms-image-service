using System;

namespace Cms.ImageService.Infrastructure.Configurations;

internal class S3ObjectStorageConfiguration
{
    public required string AccessKey { get; set; }

    public required string SecretKey { get; set; }

    public required Uri ServiceUrl { get; set; }

    public required string ImageBucketName { get; set; }

    public required bool ForcePathStyle { get; set; }
}
