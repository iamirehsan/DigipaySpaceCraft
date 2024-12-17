using DigipaySpaceCraft.Application.DTOs;

namespace DigipaySpaceCraft.Application.Interfaces.Clients
{
    public interface IWeatherApiClient
    {
        public Task<WeatherDTO> GetWeatherForecastAsync(CancellationToken token);
    }
}
