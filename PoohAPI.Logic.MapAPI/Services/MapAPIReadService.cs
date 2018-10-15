using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Infrastructure.MapAPI.APIClients;
using PoohAPI.Infrastructure.MapAPI.Models;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;

namespace PoohAPI.Logic.MapAPI.Services
{
    public class MapAPIReadService : IMapAPIReadService
    {
        private readonly IMapAPIClient mapApi;

        public MapAPIReadService(IMapAPIClient mapApi)
        {
            this.mapApi = mapApi;
        }

        public Coordinates GetMapCoordinates(string cityName, string countryName = null, string municipalityName = null)
        {
            MapCoordinates mapCoordindates;

            if (!(cityName is null))
            {
                // Use Map API
                if (!(countryName is null) && !(municipalityName is null))
                {
                    mapCoordindates = this.mapApi.GetCoordinatesLocation(cityName, municipalityName, countryName);
                }
                else if (!(countryName is null))
                {
                    mapCoordindates = this.mapApi.GetCoordinatesLocation(cityName, countryName);
                }
                else if (!(municipalityName is null))
                {
                    mapCoordindates = this.mapApi.GetCoordinatesLocation(cityName, municipalityName);
                }
                else
                {
                    mapCoordindates = this.mapApi.GetCoordinatesLocation(cityName);
                }
            }
            else
            {
                return null;
            }

            if (mapCoordindates.ResponseSucceeded)
            {
                Coordinates coordinates = new Coordinates();
                coordinates.Latitude = mapCoordindates.Latitude;
                coordinates.Longitude = mapCoordindates.Longitude;
                return coordinates;
            }

            return null;
        }
    }
}
