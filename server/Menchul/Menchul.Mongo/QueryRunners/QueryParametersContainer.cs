using Menchul.Mongo.Common;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Menchul.Mongo.QueryRunners
{
    // TODO consider to put QueryType inside
    public class QueryParametersContainer : IEnumerable
    {
        private readonly Dictionary<Type, object> _parameters = new();

        public static QueryParametersContainer From<TDocument>(IQueryParameters<TDocument> p)
            where TDocument : IDocumentRoot => new() {p};

        public void Add<TDocument>(IQueryParameters<TDocument> p) where TDocument : IDocumentRoot =>
            _parameters.Add(typeof(TDocument), p);

        public TParameter Get<TParameter, TDocument>() where TParameter : class, IQueryParameters<TDocument>, new()
            where TDocument : IDocumentRoot
        {
            var docType = typeof(TDocument);

            if (_parameters.ContainsKey(docType) && _parameters[docType] is TParameter p)
            {
                return p;
            }

            return new TParameter();
        }

        public IEnumerator GetEnumerator() => _parameters.GetEnumerator();
    }
}