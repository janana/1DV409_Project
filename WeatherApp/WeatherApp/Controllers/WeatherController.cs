using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Models;
using WeatherApp.Models.Repository;
using WeatherApp.ViewModels;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private IRepository _repository;
        public WeatherController()
            :this(new Repository())
        {
            // Empty!
        }
        public WeatherController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /Weather/

        public ActionResult Index()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Index(LocationString locationString)
        {
            try
            {
                var location = _repository.GetLocationFromString(locationString.Location);
                var weather = _repository.GetWeatherFromLocation(location);
                var forecast = new Forecast { 
                                                WeatherForecast = weather,
                                                LocationName = location.Name
                                            };

                return View("Weather", forecast); // Dont do that!!!
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
                return View("Index", locationString);
            }
           
        }
    }
}
