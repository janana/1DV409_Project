using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Models;
using WeatherApp.ViewModels;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private IWeatherService _service;

        public WeatherController()
            :this(new WeatherService())
        {
            // Empty!
        }
        public WeatherController(IWeatherService iservice)
        {
            _service = iservice;
        }

        //
        // GET: /Weather/

        public ActionResult Index()
        {
            return View("Index", new LocationString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LocationString locationString)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var location = _service.GetLocationFromString(locationString.Location);
                    var weather = _service.GetWeatherFromLocation(location);
                    var forecast = new Forecast
                    {
                        WeatherForecast = weather,
                        LocationName = location.Name
                    };

                    return View("Weather", forecast);
                }
                else
                {
                    return View("Index", locationString);
                }
                
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
                return View("Index", locationString);
            }
           
        }
    }
}
