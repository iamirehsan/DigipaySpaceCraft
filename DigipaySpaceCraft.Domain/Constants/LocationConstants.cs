namespace DigipaySpaceCraft.Domain.Constants
{
    namespace MyApp.Domain.Constants
    {
        public sealed class LocationConstants
        {

            private static readonly Lazy<LocationConstants> _instance =
                new Lazy<LocationConstants>(() => new LocationConstants());

            public static LocationConstants Instance => _instance.Value;

            private LocationConstants()
            {

                Latitude = 52.52f;
                Longitude = 13.419998f;
                Elevation = 38.0f;
                Timezone = "GMT";
                UtcOffsetSeconds = 0;
                Temperature_2m = "°C";
                Time = "iso8601";
            }

            public float Latitude { get; }
            public float Longitude { get; }
            public float Elevation { get; }
            public string Timezone { get; }
            public string TimezoneAbbreviation { get; }
            public int UtcOffsetSeconds { get; }
            public string Temperature_2m { get; }
            public string Time { get; }
        }
    }

}
