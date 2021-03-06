using System;
using Microsoft.Extensions.Caching.Memory;

namespace BattleShipApi.Persistence
{
    public interface ICacheProvider
    {
        T GetFromCache<T>(string key) where T : class;
        void SetValue<T>(string key, T value) where T : class;
        void ClearCache(string key);
    }
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        public CacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }
        public T GetFromCache<T>(string key) where T : class
        {
            _cache.TryGetValue(key, out T cachedResponse);
            return cachedResponse as T;
        }

        public void SetValue<T>(string key, T value) where T : class
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromHours(3));

            _cache.Set(key, value, options);
        }
        public void ClearCache(string key)
        {
            throw new System.NotImplementedException();
        }

    }
}