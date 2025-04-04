using Greentube.Weather.Application.WeatherApp.GetWeatherData;
using Greentube.Weather.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greentube.Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator _mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("/getWeatherData")]
    public async Task<IActionResult> GetWeatherData(string city)
    {
        string unit = GetUnitFromSessionOrReturnDefault();

        GetWeatherDataQuery query = new(city, unit);

        GetWeatherDataQueryResult result = await _mediator.Send(query);

        return Ok(ServiceResponse<GetWeatherDataQueryResult>.
            Ok(result));
    }

    [HttpPost("/setTemperatureUnit")]
    public IActionResult SetTemperatureUnit([FromBody] TemperatureUnitModel model)
    {
        HttpContext.Session.SetString(TemperatureUnitModel.SESSION_KEY, model.TemperatureUnit);

        return Ok();
    }

    private string GetUnitFromSessionOrReturnDefault()
    {
        string unit = HttpContext.Session.GetString(TemperatureUnitModel.SESSION_KEY)!;

        if (string.IsNullOrEmpty(unit))
        {
            HttpContext.Session.SetString(TemperatureUnitModel.SESSION_KEY, TemperatureUnitModel.STANDARD_UNIT);
            unit = TemperatureUnitModel.STANDARD_UNIT;
        }

        return unit;
    }
}