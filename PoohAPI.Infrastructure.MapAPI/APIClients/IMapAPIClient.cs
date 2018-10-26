using PoohAPI.Infrastructure.MapAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.MapAPI.APIClients
{
    public interface IMapAPIClient
    {
        MapCoordinates GetCoordinatesLocation(string cityName, string municipalityName, string countryName);
        MapCoordinates GetCoordinatesLocation(string cityName, string municipalityOrCountryName);
        MapCoordinates GetCoordinatesLocation(string cityName);
    }
}
