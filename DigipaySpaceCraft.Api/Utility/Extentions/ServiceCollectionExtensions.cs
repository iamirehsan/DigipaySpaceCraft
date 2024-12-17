using DigipaySpaceCraft.Application.Interfaces.CacheService;
using DigipaySpaceCraft.Application.Interfaces.Clients;
using DigipaySpaceCraft.Application.Interfaces.Services;
using DigipaySpaceCraft.Application.Services;
using DigipaySpaceCraft.Domain.Interfaces.Repositories;
using DigipaySpaceCraft.Domain.Utility.Log;
using DigipaySpaceCraft.infrastructure.Helpers.CacheService;
using DigipaySpaceCraft.infrastructure.Helpers.Log;
using DigipaySpaceCraft.infrastructure.Persistence.Context;
using DigipaySpaceCraft.infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;

namespace DigipaySpaceCraft.Api.Utility.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IWeatherRequestRepository, WeatherRequestRepository>();
            services.AddSingleton<ILoggerService, LoggerService>();
            RegisterMemoryCache(services);
            RegisterDatabase(services, databaseConnectionString);
            RegisterApiClient(services);
        }
        private static void RegisterMemoryCache(this IServiceCollection services)
        {
            services.AddSingleton<ICacheService, CacheService>();
            services.AddMemoryCache();
        }

        private static void RegisterDatabase(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(databaseConnectionString));
        }
        private static void RegisterApiClient(this IServiceCollection services)
        {

            services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
            }).AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3));
        }

    }
}
