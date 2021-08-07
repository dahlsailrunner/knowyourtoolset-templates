using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using KnowYourToolset.BackEnd.Logic;

namespace KnowYourToolset.BackEnd.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IPostalCodeLogic _postalCodeLogic;

        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(IPostalCodeLogic postalCodeLogic)
        {
            _postalCodeLogic = postalCodeLogic;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(string postalCode)
        {
            var cityName = _postalCodeLogic.GetCityForPostalCode(postalCode);

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
}
