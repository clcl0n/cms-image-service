using System.IO;

namespace Cms.ImageService.Application.Contracts;

public sealed record ImageUploadRequest(Stream FileStream, string FileName);
