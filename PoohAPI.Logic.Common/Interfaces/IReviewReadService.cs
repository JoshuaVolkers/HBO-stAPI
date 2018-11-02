using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IReviewReadService
    {
        IEnumerable<Review> GetAllReviews(int maxCount, int offset);
        Review GetReviewById(int id);
        Review GetLastReview();
        IEnumerable<int> GetListReviewIdsForUser(int userId);
        IEnumerable<ReviewPublic> GetListReviewsForCompany(int companyId);
    }
}
