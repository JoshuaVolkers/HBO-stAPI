using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    public class ReviewAnonymous
    {
        [Required]
        public double Stars { get; set; }
        public string WrittenReview { get; set; }
    }
}
