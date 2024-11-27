using Microsoft.AspNetCore.Mvc;
using KnowYourToolset.BackEnd.Api.BusinessLogic;
using Swashbuckle.AspNetCore.Annotations;

namespace KnowYourToolset.BackEnd.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class WeatherForecastController(IPostalCodeLogic postalCodeLogic) : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    [SwaggerOperation("Slightly modified version of the starter weather forecast route.",
        "Any string will return a forecast.  If you use 11111 it will generate a 500 Internal Server Error that is shielded. " +
        "A value of 22222 will return a 400 Bad Request with a message.")]
    public IEnumerable<WeatherForecast> Get(string postalCode)
    {
        var cityName = postalCodeLogic.GetCityForPostalCode(postalCode);

        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)],
            City = cityName

        })
            .ToArray();
    }
}
