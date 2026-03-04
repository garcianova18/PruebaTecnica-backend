using System.Text.Json;

namespace PruebaTecnica.Infrastructure.External.Weather;

public interface IWeatherClient
{
    Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude, string cityName, CancellationToken cancellationToken);
}

public class WeatherClient(HttpClient http) : IWeatherClient
{
    public async Task<WeatherResponse> GetWeatherAsync(double latitude, double longitude, string cityName, CancellationToken cancellationToken)
    {
        var response = await http.GetAsync(
            $"forecast?latitude={latitude}&longitude={longitude}&current_weather=true",
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var json = JsonDocument.Parse(content);
        var current = json.RootElement.GetProperty("current_weather");

        return new WeatherResponse
        {
            City = cityName,
            Temperature = current.GetProperty("temperature").GetDouble(),
            WindSpeed = current.GetProperty("windspeed").GetDouble(),
            Description = "Current weather"
        };
    }
}
