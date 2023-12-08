using Menchul.MCode.Application.Services.Interfaces;
using Menchul.Resources;
using Menchul.Resources.ReferenceBooks.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Open.Serialization.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Menchul.MCode.Application.Services
{
    public class ReferenceBookCache : IReferenceBookCache
    {
        private const string CacheKeyPrefix = "reference-book:";

        private readonly IMemoryCache _memoryCache;
        private readonly IJsonSerializer _serializer;
        private readonly ILogger<ReferenceBookCache> _logger;

        public ReferenceBookCache(IMemoryCache memoryCache, IJsonSerializer serializer,
            ILogger<ReferenceBookCache> logger)
        {
            _memoryCache = memoryCache;
            _serializer = serializer;
            _logger = logger;
        }

        public async Task<List<TReferenceBookModel>> GetAllAsync<TReferenceBookModel>()
            where TReferenceBookModel : IReferenceBookModel
        {
            var typeName = typeof(TReferenceBookModel).Name;
            var cacheKey = $"{CacheKeyPrefix}{typeName}";

            if (_memoryCache.TryGetValue(cacheKey, out var cachedValue))
            {
                _logger.LogDebug("Reference books for type {ReferenceBookType} found in the cache",
                    typeof(TReferenceBookModel).Name);

                return cachedValue as List<TReferenceBookModel>;
            }

            var items = await LoadReferenceBooks<TReferenceBookModel>();

            _memoryCache.Set(cacheKey, items);

            return items;
        }

        private ValueTask<List<TReferenceBookModel>> LoadReferenceBooks<TReferenceBookModel>()
            where TReferenceBookModel : IReferenceBookModel
        {
            _logger.LogDebug("Going to load reference books for type {ReferenceBookType}",
                typeof(TReferenceBookModel).Name);

            var data = ResourceUtils.GetReferenceBooks<TReferenceBookModel>();
            using var stream = new MemoryStream(data);

            return _serializer.DeserializeAsync<List<TReferenceBookModel>>(stream);
        }
    }
}