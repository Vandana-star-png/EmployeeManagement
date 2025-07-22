using EmployeeManagement.Data;
using EmployeeManagement.Models;
using LazyCache;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheSettings _cacheSettings;

        private static readonly List<string> EmployeeListKeys = new();

        public CacheService(IMemoryCache memoryCache, IOptions<CacheSettings> cacheOptions)
        {
            _memoryCache = memoryCache;
            _cacheSettings = cacheOptions.Value;
        }

        public async Task<EmployeeResponse> GetAllAsync(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out EmployeeResponse employeeResponse);
            return await Task.FromResult(employeeResponse);
        }

        public async Task SetAllAsync(string cacheKey, EmployeeResponse employeeResponse)
        {
            if (!EmployeeListKeys.Contains(cacheKey))
            {
                EmployeeListKeys.Add(cacheKey);
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(_cacheSettings.AbsoluteExpiration),
                SlidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpiration),
                Size = _cacheSettings.Size
            };
            _memoryCache.Set(cacheKey, employeeResponse, cacheEntryOptions);
            await Task.CompletedTask;
        }

        public async Task<Employee> GetAsync(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out Employee employee);
            return await Task.FromResult(employee);
        }

        public async Task SetAsync(string cacheKey, Employee employee)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(_cacheSettings.AbsoluteExpiration),
                SlidingExpiration = TimeSpan.FromMinutes(_cacheSettings.SlidingExpiration),
                Size = _cacheSettings.Size
            };
            _memoryCache.Set(cacheKey, employee, cacheEntryOptions);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
            await Task.CompletedTask;
        }

        public async Task RemoveAllAsync()
        {
            foreach (var cacheKey in EmployeeListKeys)
            {
                _memoryCache.Remove(cacheKey);
            }
            EmployeeListKeys.Clear();
            await Task.CompletedTask;
        }
    }
}
