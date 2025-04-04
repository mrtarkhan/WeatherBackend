using Greentube.Weather.Infrastructure.OpenWeather.Models;
using Greentube.Weather.Infrastructure.OpenWeather.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Greentube.Weather.Application.WeatherApp.GetWeatherData;

public class GetWeatherDataQueryHandler : IRequestHandler<GetWeatherDataQuery, GetWeatherDataQueryResult>
{
    private readonly IOpenWeatherService _openWeatherService;

    public GetWeatherDataQueryHandler(IOpenWeatherService openWeatherService)
    {
        _openWeatherService = openWeatherService;
    }

    public async Task<GetWeatherDataQueryResult> Handle(GetWeatherDataQuery request, CancellationToken cancellationToken)
    {
        
        WeatherData weatherData = await _openWeatherService.GetWeatherDataAsync(request.City, request.Unit, cancellationToken);

        return new GetWeatherDataQueryResult()
        {
            City = weatherData.City,
            Description = weatherData.Description,
            Icon = weatherData.Icon,
            Temprature = weatherData.Temprature,
            Unit = weatherData.Unit,
            FeelsLike = weatherData.FeelsLike,
            SunriseTime = UnixTimestampToLocalDateTime(weatherData.SunriseTime, weatherData.TimeZoneId),
            SunsetTime = UnixTimestampToLocalDateTime(weatherData.SunsetTime, weatherData.TimeZoneId),
            LocalDateTime = UnixTimestampToLocalDateTime(DateTimeOffset.UtcNow.ToUnixTimeSeconds(), weatherData.TimeZoneId),
            UtcDatetime = DateTime.UtcNow
        };
    }


    private DateTime UnixTimestampToLocalDateTime(long unixTimestamp, int timezoneOffsetSeconds)
    {
        DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
        TimeSpan offset = TimeSpan.FromSeconds(timezoneOffsetSeconds);
        DateTime localDateTime = utcDateTime.Add(offset);
        return localDateTime;
    }
}