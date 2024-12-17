using DigipaySpaceCraft.Application.DTOs;
using DigipaySpaceCraft.Application.Interfaces.CacheService;
using DigipaySpaceCraft.Application.Interfaces.Clients;
using DigipaySpaceCraft.Application.Interfaces.Services;
using DigipaySpaceCraft.Domain.Constants.MyApp.Domain.Constants;
using DigipaySpaceCraft.Domain.Entites;
using DigipaySpaceCraft.Domain.Interfaces.Repositories;
using DigipaySpaceCraft.Domain.Utility.Log;

namespace DigipaySpaceCraft.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRequestRepository _weatherRepository;
        private readonly ICacheService _cacheService;
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly ILoggerService _logger;

        public WeatherService(
            IWeatherRequestRepository weatherRepository,
            ICacheService cacheService,
            IWeatherApiClient weatherApiClient,
            ILoggerService logger)
        {
            _weatherRepository = weatherRepository ?? throw new ArgumentNullException(nameof(weatherRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _weatherApiClient = weatherApiClient ?? throw new ArgumentNullException(nameof(weatherApiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WeatherDTO> GetWeatherAsync()
        {
            _logger.WriteDebug("Starting GetWeatherAsync.");
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
            var fetchFromExternalApiTask = FetchFromExternalApiAsync(cts.Token);
            var fetchFromCacheTask = FetchFromCacheAsync();


            // Await first completion: either the API call or timeout
            var completedTask = await Task.WhenAny(fetchFromExternalApiTask, Task.Delay(4000, cts.Token));

            if (completedTask == fetchFromExternalApiTask && completedTask.IsCompletedSuccessfully)
            {
                _logger.WriteInfo("External API data fetched successfully.");
                return await fetchFromExternalApiTask;
            }

            // If API timed out, fallback to cache
            _logger.WriteWarning("External API call timed out. Fetching from cache.");
            var cacheResult = await fetchFromCacheTask;
            if (cacheResult != null)
            {
                _logger.WriteInfo("Cache data fetched successfully.");
                return cacheResult;
            }

            // Fallback to database if cache is null
            _logger.WriteInfo("Cache is empty. Fetching from database.");
            var dbResult = await FetchFromDatabaseAsync();
            if (dbResult != null)
            {
                return dbResult;
            }

            _logger.WriteWarning("No weather data available in API, cache, or database.");
            return null;

        }

        private async Task<WeatherDTO> FetchFromExternalApiAsync(CancellationToken token)
        {
            try
            {
                _logger.WriteDebug("Fetching weather data from external API...");
                var externalResult = await _weatherApiClient.GetWeatherForecastAsync(token);
                // Update cache and database in parallel
                var tasks = new[]
                {
                    _cacheService.SetOrUpdateWeatherDataInCacheAsync(externalResult),
                    AddWeatherToDatabaseAsync(externalResult)
                };
                await Task.WhenAll(tasks);

                return externalResult;
            }
            catch (OperationCanceledException)
            {
                _logger.WriteWarning("External API call was canceled (timeout).");
                throw;
            }

        }

        private async Task<WeatherDTO> FetchFromCacheAsync()
        {
            _logger.WriteDebug("Attempting to fetch weather data from cache...");
            return await _cacheService.GetWeatherDataFromCacheAsync();
        }

        private async Task<WeatherDTO> FetchFromDatabaseAsync()
        {
            _logger.WriteDebug("Fetching weather data from the database...");

            var entity = await _weatherRepository.LastOrDeafultAsync();
            if (entity == null) return null;

            var locationConstants = LocationConstants.Instance;

            return new WeatherDTO
            {
                generationtime_ms = entity.GenerationTimeMs,
                hourly_units = new Hourly_Units
                {
                    time = locationConstants.Time,
                    temperature_2m = locationConstants.Temperature_2m
                },
                hourly = new Hourly
                {
                    time = entity.HourlyWeatherData.Select(z => z.Timestamp.ToString()).ToArray(),
                    temperature_2m = entity.HourlyWeatherData.Select(z => z.Temperature).ToArray()
                },
                elevation = locationConstants.Elevation,
                latitude = locationConstants.Latitude,
                longitude = locationConstants.Longitude,
                timezone = locationConstants.Timezone,
                utc_offset_seconds = locationConstants.UtcOffsetSeconds,
                timezone_abbreviation = locationConstants.TimezoneAbbreviation
            };
        }

        private async Task AddWeatherToDatabaseAsync(WeatherDTO weatherDTO)
        {
            _logger.WriteDebug("Adding weather data to the database...");
            var weatherHourlyData = WeatherDataFactory.CreateWeatherHourlyData(weatherDTO.hourly.time, weatherDTO.hourly.temperature_2m);
            var entity = WeatherRequest.Create(weatherDTO.generationtime_ms, weatherHourlyData);
            await _weatherRepository.AddAsync(entity);
        }
    }
}
