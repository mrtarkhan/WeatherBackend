using Greentube.Weather.Application.Services;
using Greentube.Weather.Application.SharedModels;
using Greentube.Weather.Application.WeatherApp.GetWeatherData;
using Greentube.Weather.Infrastructure.OpenWeather;
using Greentube.Weather.Infrastructure.OpenWeather.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Greentube.Weather.Application;

public static class WeatherModule
{
    public static IServiceCollection AddWeatherModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenWeather(configuration);

        // decorators
        services.Decorate<IOpenWeatherService>((inner, provider) =>
            new OpenWeatherServiceWithCache(inner, provider.GetRequiredService<IDistributedCache>())
        );

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(WeatherModule).Assembly));

        services.AddTransient<IValidator<GetWeatherDataQuery>, GetWeatherDataQueryValidator>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehaviour<,>));

        return services;
    }
}