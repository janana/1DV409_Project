using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherApp.Models
{
    public class Weather
    {
        public int WeatherID
        {
            get;
            set;
        }
        public int LocationID
        {
            get;
            set;
        }
        
        public DateTime Date
        {
            get;
            set;
        }
        public string WeatherIcon
        {
            get;
            set;
        }
        public double Degree
        {
            get;
            set;
        }
    }
}
