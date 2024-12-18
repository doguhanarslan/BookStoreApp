using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.Core.CrossCuttingConcerns.Caching.Redis;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace BookStoreApp.Core.Aspects.Postsharp.CacheAspects
{
    [PSerializable]
    public class CacheAspect:MethodInterceptionAspect
    {
        //private string _cacheKey;
        //private int _duration;
        //public CacheAspect(string cacheKey, int duration= 30)
        //{
        //    _duration = duration;
        //    _cacheKey = cacheKey;
        //}

        //public override void OnInvoke(MethodInterceptionArgs args)
        //{
        //    var redisService = (RedisCacheManager)Activator.CreateInstance(typeof(RedisCacheManager), "localhost:6379");

        //    if (redisService == null)
        //    {
        //        throw new InvalidOperationException("RedisService is not available.");
        //    }

        //    // Cache'de varsa
        //    var cachedValue = redisService.GetValueAsync(_cacheKey);
        //    if (!string.IsNullOrEmpty(cachedValue))
        //    {
        //        args.ReturnValue = JsonSerializer.Deserialize<object>(cachedValue);
        //        return;
        //    }

        //    // Cache'de yoksa metodu çalıştır
        //    base.OnInvokeAsync(args);

        //    // Sonucu cache'e yaz
        //    var result = args.ReturnValue;
        //    var serializedResult = JsonSerializer.Serialize(result);
        //    redisService.SetValueAsync(_cacheKey, serializedResult, _duration);
        //}

        //public override async Task OnInvokeAsync(MethodInterceptionArgs args)
        //{

        //    var redisService = (RedisCacheManager)Activator.CreateInstance(typeof(RedisCacheManager), "localhost:6379");

        //    if (redisService == null)
        //    {
        //        throw new InvalidOperationException("RedisService is not available.");
        //    }

        //    // Cache'de varsa
        //    var cachedValue = await redisService.GetValueAsync(_cacheKey);
        //    if (!string.IsNullOrEmpty(cachedValue))
        //    {
        //        args.ReturnValue = JsonSerializer.Deserialize<object>(cachedValue);
        //        return;
        //    }

        //    // Cache'de yoksa metodu çalıştır
        //    base.OnInvokeAsync(args);

        //    // Sonucu cache'e yaz
        //    var result = args.ReturnValue;
        //    var serializedResult = JsonSerializer.Serialize(result);
        //    await redisService.SetValueAsync(_cacheKey, serializedResult, _duration);
        //}
    }
}
