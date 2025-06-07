using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Infrastructure.Services.Interfaces;

namespace Cms.ImageService.Infrastructure.Services;

internal sealed class LocalStorageService : ILocalStorageService
{
    public async Task<string> SaveFileAsync(Stream stream, CancellationToken cancellationToken)
    {
        var fullFilePath = Path.GetTempFileName();

        using var file = File.Create(fullFilePath);

        await stream.CopyToAsync(file, cancellationToken);

        stream.Position = 0;

        return fullFilePath;
    }

    public void DeleteFile(string path)
    {
        File.Delete(path);
    }
}
