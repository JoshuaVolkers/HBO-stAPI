using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models.BaseModels
{
    public class BaseLocation
    {
        public string CountryName { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
