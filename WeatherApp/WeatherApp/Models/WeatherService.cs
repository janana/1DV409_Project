using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.Models.Repository;
using WeatherApp.Models.Webservices;

namespace WeatherApp.Models
{
    public class WeatherService : IWeatherService
    {
        private IRepository _repository;

        public WeatherService()
            :this(new DbRepository())
        {
            // Empty!
        }
        public WeatherService(IRepository repository)
        {
            _repository = repository;
        }

        public Location GetLocationFromString(string locationString)
        {
            var location = _repository.GetLocationByName(locationString);
            if (location == null)
            {
                // Object is not saved in database, try to get it from webservice
                var webservice = new WeatherWebservice();
                location = webservice.GetLocationFromString(locationString);

                if (location != null)
                {
                    // Save in db!
                    _repository.AddLocation(location);
                    _repository.Save();
                }
                else
                {
                    throw new ApplicationException("Det gick inte att hitta staden varken i databasen eller via apiet.");
                }
            }
            return location;
        }
        public List<Weather> GetWeatherFromLocation(Location location)
        {
            var forecast = _repository.GetWeatherFromLocation(location);
            if (forecast == null || forecast.Count < 5 || forecast[1].Date != DateTime.Today) // Bug days switch places when saving in db
            {
                // If there are old weather objects, delete them
                forecast.ForEach(weather => _repository.DeleteWeather(weather));
                forecast = new List<Weather>();

                var webservice = new WeatherWebservice();
                forecast = webservice.GetWeatherFromLocation(location);

                if (forecast != null)
                {
                    // Save in db!
                    forecast.ForEach(w => _repository.AddWeather(w));
                    _repository.Save();
                }
                else
                {
                    throw new ApplicationException("Det gick inte att hitta väderinformation varken i databasen eller via apiet.");
                }
            }
            else if (forecast[1].Date == DateTime.Today) // Bug fix that days switch places
            {
                var today = forecast[1];
                forecast[1] = forecast[0];
                forecast[0] = today;
            }
            return forecast;
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }
        #endregion
    }
}