namespace PruebaTecnica.Infrastructure.External;

public class WeatherApiSettings
{
    public const string SectionName = "WeatherApiSettings";
    public string BaseAddress { get; set; } = string.Empty;
}

public class GeocodingApiSettings
{
    public const string SectionName = "GeocodingApiSettings";
    public string BaseAddress { get; set; } = string.Empty;
}

public class WeatherResponse
{
    public string City { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public double WindSpeed { get; set; }
    public string Description { get; set; } = string.Empty;
}
