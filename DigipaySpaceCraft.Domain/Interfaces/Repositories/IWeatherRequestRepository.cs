using DigipaySpaceCraft.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigipaySpaceCraft.Domain.Interfaces.Repositories
{
    public interface IWeatherRequestRepository
    {
        public Task<WeatherRequest> LastOrDeafultAsync();
        public Task AddAsync(WeatherRequest weatherRequest);

    }
}
