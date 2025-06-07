using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Application.Services.Interfaces;
using Cms.Contracts;
using Wolverine.Attributes;

namespace Cms.ImageService.Api.MessageHandlers;

[WolverineHandler]
public sealed class ImageMessageHandler(IImageService imageService)
{
    public async Task<ImageGetByIdResponse?> HandleAsync(
        ImageGetByIdRequest request,
        CancellationToken cancellationToken
    )
    {
        return await imageService.GetByIdAsync(request, cancellationToken);
    }
}
