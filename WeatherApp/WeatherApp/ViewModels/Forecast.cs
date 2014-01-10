using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherApp.Models;

namespace WeatherApp.ViewModels
{
    public class Forecast
    {
        public List<Weather> WeatherForecast
        {
            get;
            set;
        }
        public string LocationName
        {
            get;
            set;
        }

        public string WeatherIconURL(string weatherIcon)
        {
            return String.Format("http://openweathermap.org/img/w/{0}.png", weatherIcon);
        }
        public string GetDay(DateTime date)
        {
            return date.ToShortDateString();
        }
    }
}