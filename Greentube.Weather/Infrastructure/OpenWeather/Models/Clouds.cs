using System.Text.Json.Serialization;

namespace Greentube.Weather.Infrastructure.OpenWeather.Models;

public class Clouds
{
    [JsonPropertyName("all")]
    public int All { get; set; }
}