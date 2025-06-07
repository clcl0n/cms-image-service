using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Infrastructure.Services.Interfaces;

namespace Cms.ImageService.Infrastructure.Services;

internal sealed class ImageConvertService(ILocalStorageService localStorageService)
    : IImageConvertService
{
    public async Task<string> ConvertToWebpAsync(
        Stream stream,
        int compressionFactor,
        CancellationToken cancellationToken
    )
    {
        EnsureCompressionFactorIsInRange(compressionFactor);

        var imageToConvertPath = await localStorageService.SaveFileAsync(stream, cancellationToken);

        try
        {
            var convertedImagePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.webp");

            var startInfo = new ProcessStartInfo
            {
                FileName = "cwebp",
                Arguments =
                    $"-q {compressionFactor} \"{imageToConvertPath}\" -o \"{convertedImagePath}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            using var process = new Process { StartInfo = startInfo };

            process.Start();

            await process.WaitForExitAsync(cancellationToken);

            return convertedImagePath;
        }
        finally
        {
            if (File.Exists(imageToConvertPath))
            {
                localStorageService.DeleteFile(imageToConvertPath);
            }
        }
    }

    private static void EnsureCompressionFactorIsInRange(int compressionFactor)
    {
        if (compressionFactor < 0 || compressionFactor > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(compressionFactor),
                "Compression factor must be between 0 and 100."
            );
        }
    }
}
