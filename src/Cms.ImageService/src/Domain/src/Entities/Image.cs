using Cms.ImageService.Domain.Entities.Base;
using Cms.Contracts.Constants;

namespace Cms.ImageService.Domain.Entities;

public record Image(Guid Id, string FileName, long SizeInBytes, ImageFormat Format)
    : BaseEntity(Id);
