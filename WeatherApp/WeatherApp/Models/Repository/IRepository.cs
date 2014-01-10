using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Repository
{
    public interface IRepository : IDisposable
    {
        void AddLocation(Location location);
        void DeleteLocation(Location location);
        Location GetLocationFromString(string locationString);
        List<Weather> GetWeatherFromLocation(Location location);
        // No use for update location
        // No use for update weather/forecast



        void Save();
    }
}
