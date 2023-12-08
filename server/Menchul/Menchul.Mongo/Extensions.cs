using Convey;
using Menchul.Mongo.Common;
using Menchul.Mongo.Exceptions;
using Menchul.Mongo.QueryRunners;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Menchul.Mongo
{
    public static class Extensions
    {
        public static QueryParametersContainer ToContainer<TDocument>(this IQueryParameters<TDocument> p)
            where TDocument : IDocumentRoot => p == null ? null : new() { p };

        public static TParameter GetSafe<TParameter, TDocument>(this QueryParametersContainer container)
            where TParameter : class, IQueryParameters<TDocument>, new() where TDocument : IDocumentRoot =>
            container == null ? new TParameter() : container.Get<TParameter, TDocument>();

        public static RepositoryContextConveyBuilder WithRepositoryContext(this IConveyBuilder builder,
            MongoCollections collections, List<IRelationTracker> relationTrackers) =>
            new(builder, collections, relationTrackers);

        public static IServiceCollection AddQueryRunnerResolver<TDocument, TDefaultRunner>(this IServiceCollection serviceCollection,
            Dictionary<QueryRunnerType, Type> runners = null) where TDocument : IDocumentRoot where TDefaultRunner : class, IQueryRunner<TDocument>
            => serviceCollection.AddSingleton<QueryRunnerResolver<TDocument>>(
                provider => key =>
                {
                    if (runners == null || !key.HasValue)
                    {
                        return provider.GetService<TDefaultRunner>();
                    }

                    if (!runners.ContainsKey(key.Value))
                    {
                        throw new QueryRunnerNotResolvedException<TDocument>(key);
                    }

                    return provider.GetService(runners[key.Value]) as IQueryRunner<TDocument>;
                });


        // IProperty discriminator
        // private static void RegisterProperties(Assembly assembly)
        // {
        //     var types = assembly.GetTypes().Where(t => typeof(IProperty).IsAssignableFrom(t) && t.IsInterface == false)
        //         .ToList();
        //
        //     foreach (var type in types)
        //     {
        //
        //         var map = new BsonClassMap(type);
        //         map.AutoMap();
        //
        //         map.UnmapProperty(nameof(IProperty.PropertyType));
        //
        //         map.SetDiscriminator(((IProperty) Activator.CreateInstance(type))?.PropertyType);
        //
        //         BsonClassMap.RegisterClassMap(map);
        //     }
        // }
    }
}