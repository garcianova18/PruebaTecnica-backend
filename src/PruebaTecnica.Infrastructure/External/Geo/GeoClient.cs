using System.Text.Json;

namespace PruebaTecnica.Infrastructure.External.Geo;

public interface IGeoClient
{
    Task<(double lat, double lon, string name)> GetCoordinatesAsync(string city, CancellationToken cancellationToken);
}

public class GeoClient(HttpClient http) : IGeoClient
{
    public async Task<(double lat, double lon, string name)> GetCoordinatesAsync(string city, CancellationToken cancellationToken)
    {
        var url = $"search?name={city}&count=1";

        var response = await http.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var json = JsonDocument.Parse(content);
        var result = json.RootElement.GetProperty("results")[0];
        return (
            result.GetProperty("latitude").GetDouble(),
            result.GetProperty("longitude").GetDouble(),
            result.GetProperty("name").GetString() ?? city
        );
    }
}
