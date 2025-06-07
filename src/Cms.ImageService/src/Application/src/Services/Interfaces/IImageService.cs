using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Application.Contracts;
using Cms.Contracts;

namespace Cms.ImageService.Application.Services.Interfaces;

public interface IImageService
{
    Task<ImageGetByIdResponse?> GetByIdAsync(
        ImageGetByIdRequest request,
        CancellationToken cancellationToken
    );

    Task<ImageUploadResponse> UploadAsync(
        ImageUploadRequest request,
        CancellationToken cancellationToken
    );
}
