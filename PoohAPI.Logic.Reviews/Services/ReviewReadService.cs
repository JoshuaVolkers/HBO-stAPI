using AutoMapper;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Reviews.Services
{
    public class ReviewReadService : IReviewReadService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewReadService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public IEnumerable<Common.Models.Review> GetAllReviews(int maxCount, int offset)
        {
            var reviews = _reviewRepository.GetAllReviews(maxCount, offset);
            return _mapper.Map<IEnumerable<Common.Models.Review>>(reviews);
        }

        public Common.Models.Review GetReviewById(int id)
        {
            var query = "SELECT review_id, review_bedrijf_id, review_student_id, review_sterren" +
            ", review_geschreven, review_anoniem, review_datum, review_status" +
            ", review_status_bevestigd_door " +
            "FROM reg_reviews " +
            "WHERE review_id = @id";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            var dbReview = this._reviewRepository.GetReview(query, parameters);
            //dbReview = this._reviewRepository.GetReview(query);

            return this._mapper.Map<Common.Models.Review>(dbReview);

            //var query = string.Format("query", id);
            //var review = _reviewRepository.GetReviewById(query);

            //return _mapper.Map<Common.Models.Review>(review);
        }
    }
}