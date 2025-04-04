using MediatR;

namespace Greentube.Weather.Application.WeatherApp.GetWeatherData;

public class GetWeatherDataQuery(string city, string unit) : IRequest<GetWeatherDataQueryResult>
{
    public string City { get; private set; } = city;
    public string Unit { get; private set; } = unit;
}