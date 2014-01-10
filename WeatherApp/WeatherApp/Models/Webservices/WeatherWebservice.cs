using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using WeatherApp.Models;
using WeatherApp.Models.Repository;

namespace WeatherApp.Models.Webservices
{
    public class WeatherWebservice
    {
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
                             Latitude = decimal.Parse(geoname.Element("lat").Value, CultureInfo.InvariantCulture),
                             Longitude = decimal.Parse(geoname.Element("lng").Value, CultureInfo.InvariantCulture),
                         }).ToList();
            if (model.Count != 1)
            {
                throw new ApplicationException("Staden kunde inte hämtas från geonames api");
            }
            return model[0];
        }
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
                             Degree = Decimal.Round((decimal.Parse(time.Element("temperature").Attribute("eve").Value, CultureInfo.InvariantCulture) - (decimal)273.15), 2)
                         }).ToList();
            if (model.Count < 1)
            {
                throw new ApplicationException("Väderprognosen kunde inte hämtas från OpenWeatherMap's api");
            }

            return model;
        }
    }
}