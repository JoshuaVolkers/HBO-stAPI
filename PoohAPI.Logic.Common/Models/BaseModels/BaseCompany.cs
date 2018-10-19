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
        public Location Location { get; set; }
        public string LogoPath { get; set; }
        public double? AverageReviewStars { get; set; }
        public string Majors { get; set; }
    }
}
