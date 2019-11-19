using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugarDemo.Service;

namespace SqlSugarDemo.API.Controllers
{
    public class HomeController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        HomeService _homeService = new HomeService();
        public HomeController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            var rng = new Random();
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
             .ToArray();
            return Ok(data);
        }
        [HttpGet]
        public IActionResult GetList()
        {
            var data = _homeService.GetList();
            return Ok(data);
        }
    }
}
