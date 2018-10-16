using PoohAPI.Logic.Common.Enums;
using System;

namespace PoohAPI.Logic.Common.Models
{
    /// <summary>
    /// Class with review information for a company.
    /// </summary>
    public class Review
    {

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int Stars { get; set; }
        public string WrittenReview { get; set; }
        public int Anonymous { get; set; }
        public DateTime CreationDate { get; set; }
        public int VerifiedReview { get; set; }
        public int VerifiedBy { get; set; }
        //public string EmploymentContractPDF { get; set; }
        
        public DateTime VerificationDate { get; set; }
        
        /// <summary>
        /// Denotes whether or not the review has been Locked. Locked reviews can no longer be deleted or edited throught the API.
        /// </summary>
        public bool Locked
        {
            get
            {
                return CreationDate.AddHours(72) < DateTime.Now ? true : false;
            }
        }
    }
}
