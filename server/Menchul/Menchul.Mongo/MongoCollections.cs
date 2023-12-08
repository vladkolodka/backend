using Menchul.Mongo.Common;
using System;
using System.Collections.Generic;

namespace Menchul.Mongo
{
    public class MongoCollections : Dictionary<Type, string>
    {
        private const string RefCollectionPrefix = "ref.";
        private readonly Dictionary<string, string> _entityNames = new();

        public void Add<TDocument>(string collectionName, string entityName = null) where TDocument : IDocumentRoot
        {
            var key = typeof(TDocument);

            if (ContainsKey(key))
            {
                throw new ArgumentException($"The collection for type [{key.Name}] is already registered");
            }

            base.Add(key, collectionName);

            entityName ??= collectionName.Substring(0, collectionName.Length - 1);
            _entityNames.Add(collectionName, entityName);
        }

        public string Collection<TDocument>() where TDocument : IDocumentRoot
        {
            var key = typeof(TDocument);

            var name = base.ContainsKey(key) ? base[key] : null;

            if (name == null)
            {
                throw new ArgumentException("Collection name is not defined for specified type.", nameof(TDocument));
            }

            return name;
        }

        public string Entity(string collectionName)
        {
            var name = _entityNames.ContainsKey(collectionName) ? _entityNames[collectionName] : null;

            if (name == null)
            {
                throw new ArgumentException("Entity type is not defined for specified collection.", nameof(collectionName));
            }

            return name;
        }

        public string ReferenceCollection<TDocument>() where TDocument : IDocumentRoot
        {
            var name = Collection<TDocument>();

            if (name != null)
            {
                return RefCollectionPrefix + name;
            }

            return null;
        }
    }
}