using PoohAPI.Logic.Common.Models.BaseModels;
using System;

namespace PoohAPI.Logic.Common.Models
{
    /// <summary>
    /// Location class that contains geographical information.
    /// </summary>
    public class Location : BaseLocation
    {
        private double range;
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string HouseNumberAdditions { get; set; }
        /// <summary>
        /// The range property is used to determine the maximum circular range around the Longitude and Latitude properties.
        /// </summary>
        public double Range {
            get {
                return Math.Round(range, 2);
            }
            set {
                this.range = value;
            }
        }
    }
}
