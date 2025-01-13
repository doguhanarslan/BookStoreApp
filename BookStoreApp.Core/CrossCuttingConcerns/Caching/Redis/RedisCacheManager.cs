using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace BookStoreApp.Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _cache;
        private TimeSpan ExpireTime => TimeSpan.FromMinutes(30);
        public RedisCacheManager(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
        }


        public async Task<string> GetValueAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            return await _cache.StringSetAsync(key, value);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class
        {
            var result = await _cache.StringGetAsync(key);
            if (result.IsNull)
            {
                result = JsonSerializer.SerializeToUtf8Bytes(await action());
                await SetValueAsync(key, result);
            }
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task SetUserAsync(string username, string userData)
        {
            var key = $"user_{username}";
            await _cache.StringSetAsync(key, userData, ExpireTime);
        }

        public async Task<string?> GetUserAsync(string username)
        {
            var key = $"user_{username}";
            return await _cache.StringGetAsync(key);
        }

        public T GetOrAdd<T>(string key, Func<T> action) where T : class
        {
            var result = _cache.StringGet(key);
            if (result.IsNull)
            {
                var value = action();
                var serializedValue = JsonSerializer.Serialize(value);
                _cache.StringSet(key, serializedValue, ExpireTime);
                return value;
            }
            return JsonSerializer.Deserialize<T>(result);
        }
        //public T GetOrAddUser<T>(string username, string password, Func<T> action) where T : class
        //{
        //    var stringQuery = $"user_{username}:{password}";
        //    var result = _cache.StringGet(stringQuery);
        //    if (result.IsNull)
        //    {
        //        result = JsonSerializer.SerializeToUtf8Bytes(action());
        //        _cache.StringSet(stringQuery, result, ExpireTime);
        //    }

        //    return JsonSerializer.Deserialize<T>(result);
        //}

        public async Task Clear(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public void ClearAll()
        {
            var endpoints = _connectionMultiplexer.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }
    }
}
