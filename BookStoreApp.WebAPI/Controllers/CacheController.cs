﻿using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpPost("cache/{key}")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await _cacheService.GetValueAsync(key));
        }

        //[HttpPost("cache")]
        //public async Task<IActionResult> Post([FromBody] CacheRequestModel model)
        //{
        //    await _cacheService.SetValueAsync(model.Key, model.Value);
        //    return Ok();
        //}
        [HttpDelete("cache/{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            await _cacheService.Clear(key);
            return Ok();
        }

        [HttpDelete("clearAll")]
        public IActionResult ClearCache()
        {
            _cacheService.ClearAll();
            return Ok();
        }
    }
}
