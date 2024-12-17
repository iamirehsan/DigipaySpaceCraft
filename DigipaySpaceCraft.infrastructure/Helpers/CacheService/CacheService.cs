using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigipaySpaceCraft.infrastructure.Helpers.CacheService
{
    using DigipaySpaceCraft.Application.DTOs;
    using DigipaySpaceCraft.Application.Interfaces.CacheService;
    using DigipaySpaceCraft.Domain.Entites;
    using Microsoft.Extensions.Caching.Memory;

    public class CacheService : ICacheService
    {
        private readonly MemoryCache _cache;
        private readonly int _key = 1;

        public CacheService()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }


        public Task<WeatherDTO> GetWeatherDataFromCacheAsync()
        {
            if (_cache.TryGetValue(_key, out WeatherDTO cachedData))
            {
                return Task.FromResult(cachedData);
            }

            return Task.FromResult<WeatherDTO>(null);
        }


        public Task SetOrUpdateWeatherDataInCacheAsync(WeatherDTO data)
        {
            _cache.Set(_key, data);
            return Task.CompletedTask;
        }
    }

}
