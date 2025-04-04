using Greentube.Weather.Application.SharedModels;
using Greentube.Weather.Application.WeatherApp.GetWeatherData;
using Greentube.Weather.Infrastructure.OpenWeather;
using MediatR;

namespace Greentube.Weather.Application;

public static class WeatherModule
{
    public static IServiceCollection AddWeatherModule(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddOpenWeather(configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(WeatherModule).Assembly));

        services.AddTransient<IValidator<GetWeatherDataQuery>, GetWeatherDataQueryValidator>();
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehaviour<,>));
        
        return services;
    }
}