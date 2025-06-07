using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Domain.Entities.Base;

namespace Cms.ImageService.Infrastructure.Services.Interfaces;

public interface IDocumentDatabaseService
{
    Task UpsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : BaseEntity;

    IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : BaseEntity;

    Task SetupCollections();
}
