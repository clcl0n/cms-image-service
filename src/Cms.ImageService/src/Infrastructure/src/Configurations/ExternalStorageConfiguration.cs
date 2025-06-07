namespace Cms.ImageService.Infrastructure.Configurations;

internal class ExternalStorageConfiguration
{
    public required S3ObjectStorageConfiguration S3ObjectStorage { get; set; }
}
