using DigipaySpaceCraft.Domain.Entites;
using DigipaySpaceCraft.Domain.Interfaces.Repositories;
using DigipaySpaceCraft.infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DigipaySpaceCraft.infrastructure.Persistence.Repositories
{
    public class WeatherRequestRepository : IWeatherRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WeatherRequestRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WeatherRequest> LastOrDeafultAsync()
        {
            var result = await _dbContext.Set<WeatherRequest>().OrderBy(z => z.Id).Include(z => z.HourlyWeatherData).LastOrDefaultAsync();
            return result;
        }

        public async Task AddAsync(WeatherRequest weatherRequest)
        {
            await _dbContext.Set<WeatherRequest>().AddAsync(weatherRequest);
            await _dbContext.SaveChangesAsync();
        }

    }
}
