using Convey;
using Convey.Persistence.MongoDB;
using Convey.Types;
using Menchul.Mongo.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Menchul.Mongo
{
    public class RepositoryContextConveyBuilder : IConveyBuilder
    {
        private IConveyBuilder Builder { get; }
        private List<IRelationTracker> Trackers { get; }
        private MongoCollections Collections { get; }

        public RepositoryContextConveyBuilder(IConveyBuilder builder, MongoCollections collections,
            List<IRelationTracker> trackers)
        {
            Builder = builder;
            Collections = collections;
            Trackers = trackers;
        }

        #region DecoratedMembers

        public bool TryRegister(string name)
        {
            return Builder.TryRegister(name);
        }

        public void AddBuildAction(Action<IServiceProvider> execute)
        {
            Builder.AddBuildAction(execute);
        }

        public void AddInitializer(IInitializer initializer)
        {
            Builder.AddInitializer(initializer);
        }

        public void AddInitializer<TInitializer>() where TInitializer : IInitializer
        {
            Builder.AddInitializer<TInitializer>();
        }

        public IServiceProvider Build()
        {
            return Builder.Build();
        }

        public IServiceCollection Services => Builder.Services;

        #endregion

        public RepositoryContextConveyBuilder AddMongoRepository<TEntity, TIdentifiable>()
            where TEntity : IDocumentRoot<TIdentifiable>
        {
            var collectionName = Collections.Collection<TEntity>();

            Builder.AddMongoRepository<TEntity, TIdentifiable>(collectionName);
            Builder.Services.AddTransient<IMenchulMongoRepository<TEntity, TIdentifiable>>(sp =>
            {
                var baseRepository = sp.GetService<IMongoRepository<TEntity, TIdentifiable>>();
                return new MenchulMongoRepository<TEntity, TIdentifiable>(baseRepository, Collections, Trackers);
            });

            return this;
        }
    }
}