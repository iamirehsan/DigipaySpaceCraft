namespace DigipaySpaceCraft.Domain.Entites
{
    public class WeatherRequest
    {
        public int Id { get; private set; }
        public DateTime RequestedAt { get; private set; } = DateTime.UtcNow;
        public float GenerationTimeMs { get; private set; }
        public ICollection<WeatherHourlyData> HourlyWeatherData { get; private set; }

      
        public static WeatherRequest Create(float generationTimeMs , ICollection<WeatherHourlyData> hourlyData)
        {
            return new WeatherRequest
            {
                GenerationTimeMs = generationTimeMs,
                HourlyWeatherData = hourlyData
            };
        }

        
    }

}
