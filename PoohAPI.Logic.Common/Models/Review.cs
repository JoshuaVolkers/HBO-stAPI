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

        public double Stars { get; set; }
        public string WrittenReview { get; set; }
        public bool Anonymous { get; set; }

        public byte[] EmploymentContractPDF { get; set; }
        /// <summary>
        /// This property denotes if a review has been verified by the company/internship coordinator.
        /// </summary>
        public ReviewVerificationStatus VerifiedReview { get; set; }
        public DateTime VerificationDate { get; set; }
        public DateTime CreationDate { get; set; }
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
