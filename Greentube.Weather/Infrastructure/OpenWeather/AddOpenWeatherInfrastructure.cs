using Greentube.Weather.Infrastructure.OpenWeather.OptionModels;
using Greentube.Weather.Infrastructure.OpenWeather.Services;

namespace Greentube.Weather.Infrastructure.OpenWeather;

public static class AddOpenWeatherInfrastructure
{
    public static IServiceCollection AddOpenWeather(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<OpenWeatherOptions>(configuration.GetSection("OpenWeatherOptions"));

        services.AddHttpClient<IOpenWeatherService, OpenWeatherService>();

        return services;
    }
}