﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApp.Core.CrossCuttingConcerns.Caching
{
    public interface ICacheService
    {
        Task<string> GetValueAsync(string key);
        Task<bool> SetValueAsync(string key, string value);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class;

        T GetOrAddUser<T>(string username, string password, Func<T> action) where T : class?;
        T GetOrAdd<T>(string key, Func<T> action) where T : class;
        Task Clear(string key);
        void ClearAll();
    }
}
