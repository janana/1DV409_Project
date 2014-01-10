using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace WeatherApp.Models.Repository
{
    public class Repository : IRepository
    {
        //private _entities; // From db

        #region Location
        public void AddLocation(Location location)
        {
        
        }
        public void DeleteLocation(Location location)
        {

        }
        public Location GetLocationFromString(string locationString)
        {
            var URL = string.Format("http://api.geonames.org/search?name_equals={0}&country=Se&maxRows=1&username=janana", locationString);
            var request = (HttpWebRequest)WebRequest.Create(URL);
            var xml = String.Empty;
            using (var response = request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                xml = reader.ReadToEnd();
            }
            var doc = XDocument.Parse(xml);
            var model = (from geoname in doc.Descendants("geoname")
                           select new Location
                           {
                               LocationID = int.Parse(geoname.Element("geonameId").Value),
                               Name = geoname.Element("name").Value,
                               Latitude = double.Parse(geoname.Element("lat").Value, CultureInfo.InvariantCulture),
                               Longitude = double.Parse(geoname.Element("lng").Value, CultureInfo.InvariantCulture),
                           }).ToList();
            if (model.Count != 1)
            {
                throw new ApplicationException("Staden kunde inte hämtas från geonames api");
            }
            return model[0];
        }
        #endregion
        #region Weather
        public List<Weather> GetWeatherFromLocation(Location location)
        {
            string lat = location.Latitude.ToString();
            string lng = location.Longitude.ToString();
            lat = lat.Replace(",", ".");
            lng = lng.Replace(",", ".");

            var URL = string.Format("http://api.openweathermap.org/data/2.5/forecast/daily?lat={0}&lon={1}&cnt=5&lang=se&mode=xml&app_id=0f30ca6751e3118751f156f2e8ac847a", lat, lng);
            var request = (HttpWebRequest)WebRequest.Create(URL);
            var xml = String.Empty;
            using (var response = request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                xml = reader.ReadToEnd();
            }
            var doc = XDocument.Parse(xml);
            var model = (from time in doc.Descendants("time")
                         select new Weather
                         {
                             LocationID = location.LocationID,
                             Date = DateTime.Parse(time.Attribute("day").Value),
                             WeatherIcon = time.Element("symbol").Attribute("var").Value,
                             Degree = Math.Round((double.Parse(time.Element("temperature").Attribute("eve").Value, CultureInfo.InvariantCulture) - 273.15), 2)
                         }).ToList();
            if (model.Count < 1)
            {
                throw new ApplicationException("Väderprognosen kunde inte hämtas från OpenWeatherMap's api");
            }
            
            return model;
        }

        #endregion

        public void Save() 
        {

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