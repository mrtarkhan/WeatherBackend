namespace Greentube.Weather.Infrastructure.OpenWeather.Models;

public class WeatherData
{
    public string City { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public string Temprature { get; set; }
    public string Unit { get; set; }
    public int SunriseTime { get; set; }
    public int SunsetTime { get; set; }
    public double? FeelsLike { get; set; }
    public int TimeZoneId { get; set; }
    public int Dt { get; set; }
}