public class WeatherHourlyData
{
    public int Id { get; set; }
    public DateTime Timestamp { get; }
    public float Temperature { get; }

    public WeatherHourlyData(DateTime timestamp, float temperature)
    {
        Timestamp = timestamp;
        Temperature = temperature;
    }


}
