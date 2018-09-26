using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// Location class that contains geographical information.
    /// </summary>
    public class Location
    {
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string HouseNumberAdditions { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        /// <summary>
        /// The range property is used to determine the maximum circular range around the Longitude and Latitude properties.
        /// </summary>
        public double Range { get; set; }

    }
}
