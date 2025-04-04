namespace Greentube.Weather.Controllers;

public class TemperatureUnitModel
{
    public string TemperatureUnit { get; set; }
    public const string SESSION_KEY = "TemperatureUnit";
    public const string STANDARD_UNIT = "standard";
}