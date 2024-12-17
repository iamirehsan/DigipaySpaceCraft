using DigipaySpaceCraft.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigipaySpaceCraft.Application.Interfaces.Services
{
    public interface IWeatherService
    {
        public Task<WeatherDTO> GetWeatherAsync();
    }
}
