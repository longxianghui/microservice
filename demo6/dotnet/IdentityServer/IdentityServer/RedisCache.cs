using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Foundatio.Caching;
using IdentityServer4.Services;
using StackExchange.Redis;

namespace IdentityServer
{
    public class RedisCache<T> : ICache<T> where T : class
    {
        private readonly ICacheClient _cacheClient;

        public RedisCache(ICacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }

        public async Task<T> GetAsync(string key)
        {
            var resutl = await _cacheClient.GetAsync<T>(key);
            return resutl.Value;
        }

        public Task SetAsync(string key, T item, TimeSpan expiration)
        {
            return _cacheClient.SetAsync(key, item);
        }
    }
}