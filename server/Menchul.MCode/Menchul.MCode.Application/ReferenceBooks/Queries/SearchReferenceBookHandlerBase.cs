using Menchul.MCode.Application.ReferenceBooks.Exceptions;
using Menchul.MCode.Application.Services.Interfaces;
using Menchul.Resources.ReferenceBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.ReferenceBooks.Queries
{
    public class SearchReferenceBookHandlerBase<TBook> where TBook : IReferenceBookModel
    {
        private readonly HashSet<string> _allowedKeys;
        private readonly IReferenceBookCache _referenceBookCache;

        protected SearchReferenceBookHandlerBase(IReferenceBookCache referenceBookCache, IEnumerable<string> allowedKeys)
        {
            _referenceBookCache = referenceBookCache;
            _allowedKeys = new HashSet<string>(allowedKeys.Select(s => s.ToLower()));
        }

        private Func<TBook, bool> BuildFilteringFunction(SearchReferenceBookQuery query)
        {
            var type = typeof(TBook);
            var objectType = typeof(object);

            var filteringProperty = type.GetProperty(query.Key, BindingFlags.IgnoreCase | BindingFlags.Public
                | BindingFlags.Instance);

            if (filteringProperty == null)
            {
                return null;
            }

            var castedValues = query.Values.Select(v => Convert.ChangeType(v, filteringProperty.PropertyType))
                .ToHashSet();

            // b
            var outerParameter = Expression.Parameter(type, "b");

            // query.Values
            var queryValues = Expression.Constant(castedValues);

            // v
            var innerParameter = Expression.Parameter(objectType, "v");

            // b.<Property>
            var bProperty = Expression.Property(outerParameter, filteringProperty);

            var cast = Expression.Convert(innerParameter, filteringProperty.PropertyType);

            // v == b
            var innerBody = Expression.Equal(cast, bProperty);

            // v => v.Equals(b.<Property>)
            var innerLambda = Expression.Lambda<Func<dynamic, bool>>(innerBody, innerParameter);

            // query.Values.Any(<innerLambda>)
            var outerBody = Expression.Call(typeof(Enumerable), nameof(Enumerable.Any),
                new[] {objectType}, queryValues, innerLambda);

            // b => query.Values.Any(<innerLambda (b used inside)>)
            var outerLambda = Expression.Lambda<Func<TBook, bool>>(outerBody, outerParameter);

            return outerLambda.Compile();
        }

        protected async Task<List<TBook>> LoadAsync(SearchReferenceBookQuery query)
        {
            var items = await _referenceBookCache.GetAllAsync<TBook>();

            if (string.IsNullOrWhiteSpace(query.Key))
            {
                return items;
            }

            var filteringFunction = BuildFilteringFunction(query);

            if (!_allowedKeys.Contains(query.Key.ToLower()) || filteringFunction == null)
            {
                throw new InvalidFilteringKeyException(query.Key);
            }

            return items.Where(filteringFunction).ToList();
        }
    }
}