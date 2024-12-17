public static class WeatherDataFactory
{
    public static List<WeatherHourlyData> CreateWeatherHourlyData(string[] time, float[] temperature_2m)
    {
        var weatherHourlyDataList = new List<WeatherHourlyData>();


        if (time.Length != temperature_2m.Length)
        {
            throw new ArgumentException("The 'time' and 'temperature_2m' arrays must have the same length.");
        }

        for (int i = 0; i < time.Length; i++)
        {
            if (DateTime.TryParse(time[i], out DateTime parsedTime))
            {
                var weatherHourlyData = new WeatherHourlyData(parsedTime, temperature_2m[i]);
                weatherHourlyDataList.Add(weatherHourlyData);
            }
            else
            {
                throw new FormatException($"Invalid date format at index {i}: {time[i]}");
            }
        }

        return weatherHourlyDataList;
    }
}

