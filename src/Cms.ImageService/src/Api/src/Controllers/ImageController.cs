using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Application.Contracts;
using Cms.ImageService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cms.ImageService.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ImageController(IImageService imageService) : ControllerBase
{
    [HttpPost]
    [Route("Upload")]
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile file,
        CancellationToken cancellationToken
    )
    {
        using var fileStream = file.OpenReadStream();

        var request = new ImageUploadRequest(fileStream, file.FileName);

        var response = await imageService.UploadAsync(request, cancellationToken);

        return Ok(response);
    }
}
