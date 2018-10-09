using System.Collections.Generic;

namespace PoohAPI.Logic.Common.Models.BaseModels
{
    /// <summary>
    /// Class with basic company information.
    /// </summary>
    public class BaseCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string LogoPath { get; set; }
        public double AverageReviewStars { get; set; }
    }
}
