using DigipaySpaceCraft.Application.DTOs;
using DigipaySpaceCraft.Application.Interfaces.Clients;
using System.Net.Http.Json;

public class WeatherApiClient: IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherDTO> GetWeatherForecastAsync(CancellationToken token)
    {
        var url = "forecast?latitude=52.52&longitude=13.41&hourly=temperature_2m";
        var response = await _httpClient.GetAsync(url , token);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching weather data: {response.StatusCode}");
        }
        return await response.Content.ReadFromJsonAsync<WeatherDTO>();
    }
}
