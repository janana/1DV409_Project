using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp.Models
{
    public class Location
    {
        public int LocationID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public double Latitude
        {
            get;
            set;
        }
        public double Longitude
        {
            get;
            set;
        }
    }
}