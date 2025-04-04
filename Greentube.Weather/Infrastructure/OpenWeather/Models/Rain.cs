using System.Text.Json.Serialization;

namespace Greentube.Weather.Infrastructure.OpenWeather.Models;

public class Rain
{
    [JsonPropertyName("1h")]
    public double _1h { get; set; }
}