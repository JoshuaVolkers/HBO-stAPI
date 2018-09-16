using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// Class with basic company information.
    /// </summary>
    public class BaseCompany
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Tag> Tags { get; set; }
        public int LogoPath { get; set; }
        public double AverageReviewStars { get; set; }
    }
}
