using System.Security;

namespace ProductTest.Api.Constants
{
    public class WeatherApiOptions
    {
        public const string SecctionName = "WeatherApi";
        public string BaseUrl { get; set; } = default!;
    }
}
