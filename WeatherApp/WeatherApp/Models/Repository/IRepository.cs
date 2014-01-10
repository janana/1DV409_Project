using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models.Repository
{
    public interface IRepository : IDisposable
    {
        IQueryable<Location> FindAllLocations();
        void AddLocation(Location location);
        void DeleteLocation(Location location);
        Location GetLocationByName(string locationName);

        void AddWeather(Weather weather);
        void DeleteWeather(Weather weather);
        List<Weather> GetWeatherFromLocation(Location location);
        Weather GetWeatherByID(int weatherID);

        // No use for update location
        // No use for update weather/forecast

        void Save();
    }
}
