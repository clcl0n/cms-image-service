using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Cms.ImageService.Infrastructure.Builders;
using Cms.ImageService.Infrastructure.Configurations;
using Cms.ImageService.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Cms.ImageService.Infrastructure.Services;

internal class ExternalStorageService : IExternalStorageService, IDisposable
{
    private readonly IAmazonS3 _s3Client;

    private readonly string _imageBucketName;

    public ExternalStorageService(IOptions<ExternalStorageConfiguration> configuration)
    {
        var s3Configuration = configuration.Value.S3ObjectStorage;

        _imageBucketName = s3Configuration.ImageBucketName;

        _s3Client = new AmazonS3Client(
            S3ConfigurationHelperBuilder.BuildS3Credentials(s3Configuration),
            S3ConfigurationHelperBuilder.BuildS3Config(s3Configuration)
        );
    }

    public Task UploadImageFileAsync(string path, CancellationToken cancellationToken)
    {
        var fileName = Path.GetFileName(path);

        return _s3Client.UploadObjectFromFilePathAsync(
            _imageBucketName,
            fileName,
            path,
            null,
            cancellationToken
        );
    }

    public void Dispose()
    {
        _s3Client?.Dispose();
    }
}
