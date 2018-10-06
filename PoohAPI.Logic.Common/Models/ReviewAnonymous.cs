namespace PoohAPI.Logic.Common.Models
{
    /// <summary>
    /// Class with review about a company without user id so the reviewer is anonymous.
    /// </summary>
    public class ReviewAnonymous
    {
        public double Stars { get; set; }
        public string WrittenReview { get; set; }
        public string FirstName { get; set; }
    }
}
