using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using DigipaySpaceCraft.Application.DTOs;
using DigipaySpaceCraft.Application.Interfaces.CacheService;
using DigipaySpaceCraft.Application.Interfaces.Clients;
using DigipaySpaceCraft.Application.Interfaces.Services;
using DigipaySpaceCraft.Domain.Interfaces.Repositories;
using DigipaySpaceCraft.Domain.Utility.Log;
using Xunit;
using DigipaySpaceCraft.Application.Services;
using DigipaySpaceCraft.Domain.Entites;
using DigipaySpaceCraft.Application.Test.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace DigipaySpaceCraft.Tests
{
    public class WeatherServiceTests
    {
        private readonly Mock<IWeatherRequestRepository> _weatherRepositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IWeatherApiClient> _weatherApiClientMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly WeatherService _weatherService;

        public WeatherServiceTests()
        {
            _weatherRepositoryMock = new Mock<IWeatherRequestRepository>();
            _cacheServiceMock = new Mock<ICacheService>();
            _weatherApiClientMock = new Mock<IWeatherApiClient>();
            _loggerMock = new Mock<ILoggerService>();



            _weatherService = new WeatherService(
                _weatherRepositoryMock.Object,
                _cacheServiceMock.Object,
                _weatherApiClientMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnDataFromApi_WhenApiCompletesWithinTimeout()
        {

            var token = new CancellationTokenSource(TimeSpan.FromSeconds(4)).Token;
            var apiResult = WeatherDtoTestData.WeatherDTO;

            _weatherApiClientMock.Setup(x => x.GetWeatherForecastAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResult);

            _cacheServiceMock.Setup(x => x.GetWeatherDataFromCacheAsync())
                .ReturnsAsync((WeatherDTO)null);


            var result = await _weatherService.GetWeatherAsync();


            Assert.Equal(apiResult.generationtime_ms, result.generationtime_ms);
            _weatherApiClientMock.Verify(x => x.GetWeatherForecastAsync(It.IsAny<CancellationToken>()), Times.Once);
            _cacheServiceMock.Verify(x => x.SetOrUpdateWeatherDataInCacheAsync(apiResult), Times.Once);
            _weatherRepositoryMock.Verify(x => x.AddAsync(It.IsAny<WeatherRequest>()), Times.Once);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnDataFromCache_WhenApiTimesOut()
        {

            _weatherApiClientMock.Setup(x => x.GetWeatherForecastAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            var cacheResult = WeatherDtoTestData.WeatherDTO;

            _cacheServiceMock.Setup(x => x.GetWeatherDataFromCacheAsync())
                .ReturnsAsync(cacheResult);


            var result = await _weatherService.GetWeatherAsync();


            Assert.Equal(cacheResult.generationtime_ms, result.generationtime_ms);
            _cacheServiceMock.Verify(x => x.GetWeatherDataFromCacheAsync(), Times.Once);
            _weatherApiClientMock.Verify(x => x.GetWeatherForecastAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnDataFromDatabase_WhenCacheIsNull()
        {
            // Arrange
            _weatherApiClientMock.Setup(x => x.GetWeatherForecastAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            _cacheServiceMock.Setup(x => x.GetWeatherDataFromCacheAsync())
                .ReturnsAsync((WeatherDTO)null);

            var dbResult = WeatherDtoTestData.WeatherDTO;
            var weatherEntity = WeatherRequestTestData.WeatherRequest;

            _weatherRepositoryMock.Setup(x => x.LastOrDeafultAsync())
                .ReturnsAsync(weatherEntity);


            var result = await _weatherService.GetWeatherAsync();


            Assert.NotNull(result);
            Assert.Equal(54321, result.generationtime_ms);
            _weatherRepositoryMock.Verify(x => x.LastOrDeafultAsync(), Times.Once);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnNull_WhenAllSourcesFail()
        {

            _weatherApiClientMock.Setup(x => x.GetWeatherForecastAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            _cacheServiceMock.Setup(x => x.GetWeatherDataFromCacheAsync())
                .ReturnsAsync((WeatherDTO)null);

            _weatherRepositoryMock.Setup(x => x.LastOrDeafultAsync())
                .ReturnsAsync((WeatherRequest)null);


            var result = await _weatherService.GetWeatherAsync();


            Assert.Null(result);
            _loggerMock.Verify(x => x.WriteWarning(It.IsAny<string>()), Times.AtLeastOnce);
        }
    }
}
