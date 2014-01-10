using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using WeatherApp.Models.DataModels;
using WeatherApp.Models.Webservices;

namespace WeatherApp.Models.Repository
{
    public class DbRepository : IRepository
    {
        private jb222qp_weatherEntities _entities = new jb222qp_weatherEntities();

        #region Location
        public IQueryable<Location> FindAllLocations()
        {
            return _entities.Locations.AsQueryable();
        }
        public void AddLocation(Location location)
        {
            _entities.Locations.Add(location);
        }
        public void DeleteLocation(Location location)
        {
            _entities.Locations.Remove(location);
        }
        public Location GetLocationByName(string locationName)
        {
            return FindAllLocations().SingleOrDefault(loc => loc.Name == locationName);
        }
        #endregion
        #region Weather
        public void AddWeather(Weather weather)
        {
            _entities.Weathers.Add(weather);
        }
        public void DeleteWeather(Weather weather)
        {
            _entities.Weathers.Remove(weather);
        }
        public List<Weather> GetWeatherFromLocation(Location location)
        {
            return _entities.Weathers.Where(w => w.LocationID == location.LocationID).ToList();
        }
        public Weather GetWeatherByID(int weatherID)
        {
            return _entities.Weathers.Find(weatherID);
        }

        #endregion

        public void Save() 
        {
            _entities.SaveChanges();
        }

        #region IDisposable
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //_entities.Dispose();
                }
            }
            _disposed = true;
        }
        #endregion
    }
}