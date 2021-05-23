using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace SearchEngine
{
    public static class SearchResultCache
    {
        private static readonly MemoryCache _memoryCache = MemoryCache.Default;

        public static async Task<string?> GetAsync(string url, Func<Task<string?>> onMiss)
        {
            string cacheKey = $"RankCacheKey_{url}";
            var value = _memoryCache.Get(cacheKey);
            if (value == null)
            {
                var htmlResult = await onMiss();
                _memoryCache.Set(cacheKey, htmlResult, DateTimeOffset.UtcNow + TimeSpan.FromHours(1));
                return htmlResult;
            }
            return value as string;
        }
    }
}