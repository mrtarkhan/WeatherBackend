namespace Greentube.Weather.Application.WeatherApp.GetWeatherData;

public class GetWeatherDataQueryResult
{
    public string City { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public string Temprature { get; set; }
    public string Unit { get; set; }
    public DateTime SunriseTime { get; set; }
    public DateTime SunsetTime { get; set; }
    public DateTime UtcDatetime { get; set; }
    public DateTime LocalDateTime { get; set; }
    public double? FeelsLike { get; set; }
}