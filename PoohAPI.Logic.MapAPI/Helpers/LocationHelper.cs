using PoohAPI.Logic.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.MapAPI.Services;
using PoohAPI.Logic.Common.Classes;
using PoohAPI.Logic.Common.Interfaces;

namespace PoohAPI.Logic.MapAPI.Helpers
{
    public class LocationHelper
    {
        public void AddLocationFilter(ref Dictionary<string, object> parameters, IMapAPIReadService mapAPIReadService, ref IQueryBuilder queryBuilder, char tablename, string columnname, string countryName = null,
                                        string municipalityName = null, string cityName = null, int? locationRange = null)
        {
            if (!(cityName is null) && !(locationRange is null))
            {
                // Use Map API
                Coordinates coordinates = mapAPIReadService.GetMapCoordinates(cityName, countryName, municipalityName);

                if (!(coordinates is null))
                {
                    parameters.Add("@latitude", coordinates.Latitude);
                    parameters.Add("@longitude", coordinates.Longitude);
                    parameters.Add("@rangeKm", locationRange);

                    queryBuilder.AddSelect(String.Format(@"(
                        6371 * acos(
                          cos(radians(@latitude))
                          * cos(radians({0}.{1}_breedtegraad))
                          * cos(radians({0}.{1}_lengtegraad) - radians(@longitude))
                          + sin(radians(@latitude))
                          * sin(radians({0}.{1}_breedtegraad))
                        )) as distance", tablename, columnname));
                    queryBuilder.AddHaving("distance < @rangeKm");
                }

                else
                {
                    // Find matches in database
                    AddLocationFilterWithoutCoordinates(ref parameters, countryName, cityName, ref queryBuilder);
                }
            }
            else
            {
                // Find matches in database
                AddLocationFilterWithoutCoordinates(ref parameters, countryName, cityName, ref queryBuilder);
            }
        }


        private void AddLocationFilterWithoutCoordinates(ref Dictionary<string, object> parameters, string countryName, string cityName, ref IQueryBuilder queryBuilder)
        {
            if (cityName != null)
            {
                queryBuilder.AddWhere("b.bedrijf_vestiging_plaats = @cityName");
                parameters.Add("@cityName", cityName);
            }

            if (countryName != null)
            {
                queryBuilder.AddWhere("l.land_naam = @countryName");
                parameters.Add("@countryName", countryName);
            }
        }
    }
}
