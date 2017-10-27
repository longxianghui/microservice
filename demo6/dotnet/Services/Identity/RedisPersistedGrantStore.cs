using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundatio.Caching;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;

namespace Identity
{
    public class RedisPersistedGrantStore : IPersistedGrantStore
    {
        private readonly ICacheClient _cacheClient;
        private readonly IConfiguration _configuration;

        public RedisPersistedGrantStore(ICacheClient cacheClient, IConfiguration configuration)
        {
            _cacheClient = cacheClient;
            _configuration = configuration;
        }

        /// <summary>Stores the grant.</summary>
        /// <param name="grant">The grant.</param>
        /// <returns></returns>
        public Task StoreAsync(PersistedGrant grant)
        {
            var accessTokenLifetime = double.Parse(_configuration.GetConnectionString("accessTokenLifetime"));
            var timeSpan = TimeSpan.FromSeconds(accessTokenLifetime);
            _cacheClient?.SetAsync(grant.Key, grant, timeSpan);
            return Task.CompletedTask;
        }

        /// <summary>Gets the grant.</summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Task<PersistedGrant> GetAsync(string key)
        {
            if (_cacheClient.ExistsAsync(key).Result)
            {
                var ss = _cacheClient.GetAsync<PersistedGrant>(key).Result;
                return Task.FromResult<PersistedGrant>(_cacheClient.GetAsync<PersistedGrant>(key).Result.Value);
            }
            return Task.FromResult<PersistedGrant>((PersistedGrant)null);
        }

        /// <summary>Gets all grants for a given subject id.</summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns></returns>
        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = _cacheClient.GetAllAsync<PersistedGrant>().Result.Values;
            return Task.FromResult<IEnumerable<PersistedGrant>>(persistedGrants
                .Where(x => x.Value.SubjectId == subjectId).Select(x => x.Value));
        }

        /// <summary>Removes the grant by key.</summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Task RemoveAsync(string key)
        {
            _cacheClient?.RemoveAsync(key);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes all grants for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <returns></returns>
        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            _cacheClient.RemoveAllAsync();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes all grants of a give type for a given subject id and client id combination.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var persistedGrants = _cacheClient.GetAllAsync<PersistedGrant>().Result.Values
                .Where(x => x.Value.SubjectId == subjectId && x.Value.ClientId == clientId &&
                            x.Value.Type == type).Select(x => x.Value);
            foreach (var item in persistedGrants)
            {
                _cacheClient?.RemoveAsync(item.Key);
            }
            return Task.CompletedTask;
        }
    }
}
