using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public interface IWeatherService : IDisposable
    {
        Location GetLocationFromString(string locationString);
        List<Weather> GetWeatherFromLocation(Location location);
    }
}
