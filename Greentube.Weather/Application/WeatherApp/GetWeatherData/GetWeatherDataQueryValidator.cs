using Greentube.Weather.Application.SharedModels;

namespace Greentube.Weather.Application.WeatherApp.GetWeatherData;

public class GetWeatherDataQueryValidator : IValidator<GetWeatherDataQuery>
{

    private readonly ILogger<GetWeatherDataQueryValidator> _logger;

    public GetWeatherDataQueryValidator(ILogger<GetWeatherDataQueryValidator> logger)
    {
        _logger = logger;
    }

    public void Validate(GetWeatherDataQuery inputModel)
    {
        if (string.IsNullOrWhiteSpace(inputModel.Unit))
        {
            _logger.LogTrace($"unit {inputModel.Unit} is missing");
            throw new ArgumentException("unit is invalid");
        }

        var units = new string[] { "standard", "metric", "imperial" };

        if (units.Contains(inputModel.Unit.ToLower()) == false)
        {
            _logger.LogTrace($"unit {inputModel.Unit} is not in the list");
            throw new ArgumentException("unit is invalid");
        }
        
        if (string.IsNullOrWhiteSpace(inputModel.City))
        {
            _logger.LogTrace($"city {inputModel.City} is missing");
            throw new ArgumentException("city is invalid");
        }

        // TODO: this should be a service so we can also internationalize it, for example we can accept Wien or maybe create a coding for cities
        var cities = new string[] { "london", "vienna", "ljubljana", "belgrade", "valletta"};
        
        if (cities.Contains(inputModel.City.ToLower()) == false)
        {
            _logger.LogTrace($"city {inputModel.City} is not in the list");
            throw new ArgumentException("city is not supported yet!");
        }

    }
}