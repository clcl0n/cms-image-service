using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cms.ImageService.Infrastructure.Services.Interfaces;

public interface IImageConvertService
{
    Task<string> ConvertToWebpAsync(
        Stream stream,
        int quality,
        CancellationToken cancellationToken
    );
}
