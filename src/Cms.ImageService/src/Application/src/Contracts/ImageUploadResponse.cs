using System;
using Cms.Contracts.Constants;

namespace Cms.ImageService.Application.Contracts;

public sealed record ImageUploadResponse(
    Guid Id,
    string FileName,
    long SizeInBytes,
    ImageFormat Format
);
