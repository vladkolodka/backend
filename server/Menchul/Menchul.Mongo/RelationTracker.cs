using Menchul.Mongo.Common;
using Menchul.Mongo.Documents;
using Menchul.Mongo.Exceptions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menchul.Mongo
{
    public interface IRelationTracker
    {
        bool Permanent { get; }
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IRelationTracker<TDocument, in TId> : IRelationTracker
        where TDocument : IDocumentRoot<TId>
    {
        Task BindAsync(TId id, IList<IDocumentRef> refs, IMongoDatabase database);
        Task UnbindAsync(TId id, IList<IDocumentRef> refs, IMongoDatabase database);
    }

    public class RelationTracker<TDocument, TId, TParentDocument, TParentId> : IRelationTracker<TDocument, TId>
        where TDocument : IDocumentRoot<TId>
        where TParentDocument : IDocumentRoot<TParentId>
    {
        private readonly string _refsCollectionName;
        private readonly string _collectionName;

        public bool Permanent { get; }

        public RelationTracker(MongoCollections collections, bool isPermanent = false)
        {
            Permanent = isPermanent;
            _refsCollectionName = collections.ReferenceCollection<TParentDocument>();
            _collectionName = collections.Collection<TDocument>();
        }

        public Task BindAsync(TId id, IList<IDocumentRef> refs, IMongoDatabase database)
        {
            var parentIds = refs.OfType<DocumentRef<TParentDocument, TParentId>>()
                .Select(r => r.RefId).Distinct().ToList();

            if (!parentIds.Any())
            {
                return Task.CompletedTask;
            }

            var refsCollection = database.GetCollection<ReferenceDocument<TParentId>>(_refsCollectionName);

            var updateDefinition = Permanent
                ? Builders<ReferenceDocument<TParentId>>.Update.Set(r => r.Permanent, true)
                : Builders<ReferenceDocument<TParentId>>.Update.AddToSet(d => d.Refs[_collectionName], id);

            if (parentIds.Count == 1)
            {
                var parentId = parentIds.First();

                return refsCollection.UpdateOneAsync(d => d.Id.Equals(parentId), updateDefinition,
                    new UpdateOptions {IsUpsert = true});
            }

            var updates = parentIds.Select(pId => new UpdateOneModel<ReferenceDocument<TParentId>>(
                new ExpressionFilterDefinition<ReferenceDocument<TParentId>>(d => d.Id.Equals(pId)),
                updateDefinition
            ) {IsUpsert = true});

            return refsCollection.BulkWriteAsync(updates);
        }

        public Task UnbindAsync(TId id, IList<IDocumentRef> refs, IMongoDatabase database)
        {
            if (Permanent)
            {
                throw new EntityReferencedException();
            }

            var parentIds = refs.OfType<DocumentRef<TParentDocument, TParentId>>()
                .Select(r => r.RefId).Distinct().ToList();

            if (!parentIds.Any())
            {
                return Task.CompletedTask;
            }

            var refsCollection = database.GetCollection<ReferenceDocument<TParentId>>(_refsCollectionName);

            var updateDefinition =
                Builders<ReferenceDocument<TParentId>>.Update.Pull(d => d.Refs[_collectionName], id);
            var cleanupDefinition =
                Builders<ReferenceDocument<TParentId>>.Update.Unset(d => d.Refs[_collectionName]);

            var updates = parentIds.SelectMany(pId => new WriteModel<ReferenceDocument<TParentId>>[]
            {
                new UpdateOneModel<ReferenceDocument<TParentId>>(
                    new ExpressionFilterDefinition<ReferenceDocument<TParentId>>(d => d.Id.Equals(pId)),
                    updateDefinition
                ) {IsUpsert = true},
                new UpdateOneModel<ReferenceDocument<TParentId>>(
                    new ExpressionFilterDefinition<ReferenceDocument<TParentId>>(d =>
                        d.Id.Equals(pId) && d.Refs.ContainsKey(_collectionName) && !d.Refs[_collectionName].Any()),
                    cleanupDefinition)
            });

            return refsCollection.BulkWriteAsync(updates);
        }
    }
}