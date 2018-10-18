using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models.InputModels
{
    public class ReviewUpdateInput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Stars { get; set; }
        [Required]
        public string WrittenReview { get; set; }
        [Required]
        public int Anonymous { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public int VerifiedReview { get; set; }
        [Required]
        public int VerifiedBy { get; set; }        
    }
}
