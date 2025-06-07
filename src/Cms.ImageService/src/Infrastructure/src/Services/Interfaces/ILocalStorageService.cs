using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cms.ImageService.Infrastructure.Services.Interfaces;

public interface ILocalStorageService
{
    Task<string> SaveFileAsync(Stream stream, CancellationToken cancellationToken);

    void DeleteFile(string path);
}
