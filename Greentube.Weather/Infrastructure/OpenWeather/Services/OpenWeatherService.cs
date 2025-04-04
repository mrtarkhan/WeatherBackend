using System.Globalization;
using System.Text.Json;
using Greentube.Weather.Infrastructure.OpenWeather.Models;
using Greentube.Weather.Infrastructure.OpenWeather.OptionModels;
using Microsoft.Extensions.Options;

namespace Greentube.Weather.Infrastructure.OpenWeather.Services;

public class OpenWeatherService : IOpenWeatherService
{
    private readonly ILogger<OpenWeatherService> _logger;
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _openWeatherOptions;

    private const string LOG_TEMPLATE = "OpenWeatherService: {city}, {unit}, {message}";
    
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public OpenWeatherService(ILogger<OpenWeatherService> logger, HttpClient httpClient, IOptions<OpenWeatherOptions> openWeatherOptions)
    {
        _httpClient = httpClient;
        _logger = logger;
        _openWeatherOptions = openWeatherOptions?.Value ?? throw new ArgumentNullException(nameof(openWeatherOptions), "openWeatherOptions is required");
    }

    public async Task<WeatherData> GetWeatherDataAsync(string city, string unit, CancellationToken cancellationToken)
    {
        string url = $"{_openWeatherOptions.BaseUrl}?q={city}&appid={_openWeatherOptions.ApiKey}&units={unit}";

        _logger.LogTrace(LOG_TEMPLATE, city, unit, $"calling at {DateTime.UtcNow.Ticks}");

        HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);

        _logger.LogTrace(LOG_TEMPLATE, city, unit, $"response at {DateTime.UtcNow.Ticks}");
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(LOG_TEMPLATE, city, unit, $"http request failed {response.StatusCode} - {response.ReasonPhrase}");
            throw new Exception($"Error fetching weather data: {response.StatusCode}");
        }

        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogTrace(LOG_TEMPLATE, city, unit, $"responseContent: {responseContent}");

        WeatherResponse? result;
        
        try
        {
            result = JsonSerializer.Deserialize<WeatherResponse>(responseContent, _jsonOptions);
        }
        catch (Exception e)
        {
            _logger.LogTrace(LOG_TEMPLATE, city, unit, $"exception on deserializing {e.Message}");
            _logger.LogError(e, "deserializing error");
            throw;
        }

        return new WeatherData
        {
            City = result?.Name ?? "Unknown",
            Unit = unit,
            TimeZoneId = result?.Timezone ?? 0,
            Description = result?.Weather?[0]?.Description ?? "Not Providable",
            Icon = $"https://openweathermap.org/img/wn/{result?.Weather?[0]?.Icon}@2x.png",
            Temprature = result?.Main?.Temp.ToString(CultureInfo.InvariantCulture) ?? "Not Providable",
            SunriseTime = result?.Sys?.Sunrise ?? 0,
            SunsetTime = result?.Sys?.Sunset ?? 0,
            FeelsLike = result?.Main?.FeelsLike,
            Dt = result?.Dt ?? 0
        };
    }
}