using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    // Class with review information for a company
    public class Review
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public double Stars { get; set; }
        public string WrittenReview { get; set; }
    }
}
