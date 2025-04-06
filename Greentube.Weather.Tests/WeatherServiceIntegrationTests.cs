using System.Net;
using Greentube.Weather.Application.WeatherApp.GetWeatherData;
using Greentube.Weather.Models;
using Newtonsoft.Json;

namespace Greentube.Weather.Tests;

public class WeatherServiceIntegrationTests : IClassFixture<WebAppFactory>
{

    private readonly HttpClient _httpClient;
    
    public WeatherServiceIntegrationTests(WebAppFactory webAppFactory)
    {
        _httpClient = webAppFactory.CreateClient();
    }
    
    [Fact]
    public void Should_call_getWeatherData_and_get_ok_response()
    {
        // Arrange
        string city = "Vienna";
        string url = $"/getWeatherData?city={city}";

        // Act
        HttpResponseMessage response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
        string stringResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        ServiceResponse<GetWeatherDataQueryResult> data = JsonConvert.DeserializeObject<ServiceResponse<GetWeatherDataQueryResult>>(stringResult)!;
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(200, data.Status);
        Assert.Equal(city, data.Data.City);
        Assert.Equal("standard", data.Data.Unit);
        
    }
    
    [Fact]
    public void Should_call_getWeatherData_and_return_BadRequest()
    {
        // Arrange
        string city = "Test";
        string url = $"/getWeatherData?city={city}";

        // Act
        HttpResponseMessage response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
        string stringResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        ServiceResponse<GetWeatherDataQueryResult> data = JsonConvert.DeserializeObject<ServiceResponse<GetWeatherDataQueryResult>>(stringResult)!;
        
        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        Assert.Equal(400, data.Status);
    }
}