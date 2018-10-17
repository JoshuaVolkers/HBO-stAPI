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
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
