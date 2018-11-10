using System;
using System.ComponentModel.DataAnnotations;

namespace PoohAPI.RequestModels
{
    /// <summary>
    /// Class with review information for a company.
    /// </summary>
    public class ReviewPost
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int Stars { get; set; }   
        [Required]
        public string WrittenReview { get; set; }
        [Required]
        public int Anonymous { get; set; }
    }
}
