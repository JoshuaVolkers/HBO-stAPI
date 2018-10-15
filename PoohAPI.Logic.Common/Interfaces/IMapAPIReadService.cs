using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Models;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IMapAPIReadService
    {
        Coordinates GetMapCoordinates(string cityName, string countryName = null, string municipalityName = null);
    }
}
