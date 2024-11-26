using Microsoft.AspNetCore.Mvc;
using KnowYourToolset.BackEnd.Api.BusinessLogic;

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
