using DigipaySpaceCraft.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigipaySpaceCraft.Application.Interfaces.CacheService
{
    public interface ICacheService
    {
        public Task<WeatherDTO> GetWeatherDataFromCacheAsync();
        public Task SetOrUpdateWeatherDataInCacheAsync(WeatherDTO data);

    }
}
