using Greentube.Weather.Infrastructure.OpenWeather.Models;

namespace Greentube.Weather.Infrastructure.OpenWeather.Services;

public interface IOpenWeatherService
{
    Task<WeatherData> GetWeatherDataAsync(string city, string unit, CancellationToken cancellationToken);
}