using AutoMapper;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.InputModels;
using System.Collections.Generic;

namespace PoohAPI.Logic.Reviews.Services
{
    public class ReviewCommandService : IReviewCommandService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewCommandService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public void DeleteReview(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", id);

            string query = "DELETE FROM reg_reviews WHERE review_id = @id;";

            _reviewRepository.DeleteReview(query, parameters);
        }

        public Review UpdateReview(ReviewUpdateInput ReviewInput)
        {
            throw new System.NotImplementedException();
        }
    }
}
