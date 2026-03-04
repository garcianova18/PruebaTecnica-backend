using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Application.Common.Models;
using PruebaTecnica.Infrastructure.External;
using PruebaTecnica.Infrastructure.External.Geo;
using PruebaTecnica.Infrastructure.External.Weather;

namespace PruebaTecnica.Api.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(IGeoClient geoClient, IWeatherClient weatherClient) : ControllerBase
{
    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeather(string city, CancellationToken cancellationToken)
    {
        var (lat, lon, name) = await geoClient.GetCoordinatesAsync(city, cancellationToken);
        var weather = await weatherClient.GetWeatherAsync(lat, lon, name, cancellationToken);
        return Ok(ApiResponse<WeatherResponse>.Success(weather));
    }
}
