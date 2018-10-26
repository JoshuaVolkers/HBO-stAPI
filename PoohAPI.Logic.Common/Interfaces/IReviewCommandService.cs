using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IReviewCommandService
    {
        Review UpdateReview(ReviewUpdateInput ReviewInput);
        void DeleteReview(int id);
        Review PostReview(int companyId, int userId, int stars, string writtenReview, int anonymous);
    }
}
