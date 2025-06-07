using System.Threading;
using System.Threading.Tasks;

namespace Cms.ImageService.Infrastructure.Services.Interfaces;

public interface IExternalStorageService
{
    Task UploadImageFileAsync(string path, CancellationToken cancellationToken);
}
