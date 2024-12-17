using DigipaySpaceCraft.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigipaySpaceCraft.Application.Test.TestData
{
    public static class WeatherRequestTestData
    {
        public static WeatherRequest WeatherRequest => WeatherRequest.Create(54321, new[] { new WeatherHourlyData(DateTime.Now, 1) });
    }
}
