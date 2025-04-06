using System.Text;
using System.Text.Json;
using Greentube.Weather.Infrastructure.OpenWeather.Models;
using Greentube.Weather.Infrastructure.OpenWeather.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Greentube.Weather.Application.Services;

public class OpenWeatherServiceWithCache : IOpenWeatherService
{
    private readonly IOpenWeatherService _openWeatherService;
    private readonly IDistributedCache _distributedCache;
    
    public OpenWeatherServiceWithCache(IOpenWeatherService openWeatherService, IDistributedCache distributedCache)
    {
        _openWeatherService = openWeatherService;
        _distributedCache = distributedCache;
    }

    public async Task<WeatherData> GetWeatherDataAsync(string city, string unit, CancellationToken cancellationToken)
    {
        string cacheKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(city + unit));

        string cacheResult = await _distributedCache.GetStringAsync(cacheKey, cancellationToken) ?? string.Empty;

        if (string.IsNullOrEmpty(cacheResult) == false)
        {
            try
            {
                return JsonSerializer.Deserialize<WeatherData>(cacheResult)!;
            }
            catch (Exception e)
            {
                await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
            }
        }
        
        WeatherData result = await _openWeatherService.GetWeatherDataAsync(city, unit, cancellationToken);

        await _distributedCache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result)), token: cancellationToken);

        return result;

    }
}