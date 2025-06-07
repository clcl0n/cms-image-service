using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Application.Contracts;
using Cms.ImageService.Application.Services.Interfaces;
using Cms.ImageService.Domain.Entities;
using Cms.ImageService.Infrastructure.Services.Interfaces;
using Cms.Contracts.Constants;
using Cms.Contracts;
using MongoDB.Driver.Linq;

namespace Cms.ImageService.Application.Services;

internal sealed class ImageService(
    ILocalStorageService localStorageService,
    IExternalStorageService externalStorageService,
    IImageConvertService imageConvertService,
    IDocumentDatabaseService documentDatabaseService
) : IImageService
{
    public async Task<ImageGetByIdResponse?> GetByIdAsync(
        ImageGetByIdRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await documentDatabaseService
            .GetQueryable<Image>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return result is null
            ? null
            : new ImageGetByIdResponse(
                result.Id,
                result.FileName,
                result.SizeInBytes,
                result.Format
            );
    }

    public async Task<ImageUploadResponse> UploadAsync(
        ImageUploadRequest request,
        CancellationToken cancellationToken
    )
    {
        string? outputFilePath = null;

        try
        {
            outputFilePath = await imageConvertService.ConvertToWebpAsync(
                request.FileStream,
                100,
                cancellationToken
            );

            var outputFileInfo = new FileInfo(outputFilePath);

            var image = new Image(
                Guid.NewGuid(),
                outputFileInfo.Name,
                outputFileInfo.Length,
                ImageFormat.Webp
            );

            await externalStorageService.UploadImageFileAsync(outputFilePath, cancellationToken);

            await documentDatabaseService.UpsertAsync(image, cancellationToken);

            return new ImageUploadResponse(
                image.Id,
                image.FileName,
                image.SizeInBytes,
                image.Format
            );
        }
        finally
        {
            if (outputFilePath is not null)
            {
                localStorageService.DeleteFile(outputFilePath);
            }
        }
    }
}
