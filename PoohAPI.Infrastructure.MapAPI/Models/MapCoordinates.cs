using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.MapAPI.Models
{
    public class MapCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool ResponseSucceeded { get; set; }
    }
}
