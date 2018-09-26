using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// Class with review information for a company.
    /// </summary>
    public class ReviewPost
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Range(1.0, 5.0)]
        [Required]
        public double Stars { get; set; }
        public string WrittenReview { get; set; }
        public bool Anonymous { get; set; }
        [Required]
        public byte[] EmploymentContractPDF { get; set; }
    }
}
