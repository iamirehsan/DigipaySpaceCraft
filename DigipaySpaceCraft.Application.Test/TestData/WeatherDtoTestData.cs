using DigipaySpaceCraft.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigipaySpaceCraft.Application.Test.TestData
{
    public static class WeatherDtoTestData
    {
        public static WeatherDTO WeatherDTO =>
              new WeatherDTO
              {
                  latitude = 52.52f,
                  longitude = 13.41f,
                  generationtime_ms = 123.45f,
                  utc_offset_seconds = 0,
                  timezone = "GMT",
                  timezone_abbreviation = "GMT",
                  elevation = 38.0f,
                  hourly_units = new Hourly_Units
                  {
                      time = "iso8601",
                      temperature_2m = "°C"
                  },
                  hourly = new Hourly
                  {
                      time = new[]
                {
                    "2024-12-15T00:00",
                    "2024-12-15T01:00",
                    "2024-12-15T02:00",
                    "2024-12-15T03:00",
                    "2024-12-15T04:00"
                },
                      temperature_2m = new[]
                {
                    0.2f, -0.4f, -0.8f, -1.2f, -1.4f
                }
                  }
              };
    }
}
