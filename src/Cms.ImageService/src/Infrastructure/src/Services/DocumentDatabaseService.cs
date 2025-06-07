using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cms.ImageService.Domain.Entities;
using Cms.ImageService.Domain.Entities.Base;
using Cms.ImageService.Infrastructure.Services.Interfaces;
using Cms.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Cms.ImageService.Infrastructure.Services;

internal sealed class DocumentDatabaseService : IDocumentDatabaseService
{
    private readonly FrozenDictionary<Type, string> _entityToCollectionNameMap;

    private readonly IMongoDatabase _database;

    private readonly MongoClient _client;

    public DocumentDatabaseService(IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        var url = MongoUrl.Create(configuration.GetConnectionString("MongoDb"));
        var clientSettings = MongoClientSettings.FromUrl(url);

        clientSettings.ClusterConfigurator = cb =>
            cb.Subscribe(
                new DiagnosticsActivityEventSubscriber(
                    new InstrumentationOptions { CaptureCommandText = true }
                )
            );

        _client = new MongoClient(clientSettings);

        _database = _client.GetDatabase(url.DatabaseName);
        _entityToCollectionNameMap = GetEntityToCollectionNameMap().ToFrozenDictionary();
    }

    public async Task UpsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        where TEntity : BaseEntity
    {
        if (
            _entityToCollectionNameMap.TryGetValue(typeof(TEntity), out var collectionName) is false
        )
        {
            throw new NotImplementedException(
                $"Missing collection name config for entity: {typeof(TEntity)}"
            );
        }

        var collection = _database.GetCollection<TEntity>(collectionName);

        await collection.ReplaceOneAsync(
            x => x.Id == entity.Id,
            entity,
            new ReplaceOptions { IsUpsert = true },
            cancellationToken
        );
    }

    public IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : BaseEntity
    {
        if (_entityToCollectionNameMap.TryGetValue(typeof(TEntity), out var collectionName))
        {
            var collection = _database.GetCollection<TEntity>(collectionName);

            return collection.AsQueryable();
        }

        throw new NotImplementedException(
            $"Missing collection name config for entity: {typeof(TEntity)}"
        );
    }

    public async Task SetupCollections()
    {
        var collections = await _database.ListCollectionNames().ToListAsync();

        foreach (var collectionName in _entityToCollectionNameMap.Values)
        {
            await CreateCollectionIfNotExistsAsync(collectionName, collections);
        }
    }

    private async Task CreateCollectionIfNotExistsAsync(
        string collectionName,
        List<string> existingCollections
    )
    {
        if (existingCollections.Contains(collectionName) is false)
        {
            await _database.CreateCollectionAsync(collectionName);
        }
    }

    private static Dictionary<Type, string> GetEntityToCollectionNameMap()
    {
        var types = new[] { typeof(Image) };

        return types.ToDictionary(type => type, type => type.NameToLowerInvariant());
    }
}
