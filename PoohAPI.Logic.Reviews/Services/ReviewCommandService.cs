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
        private readonly IReviewReadService _reviewReadService;

        public ReviewCommandService(IReviewRepository reviewRepository, IMapper mapper, IReviewReadService reviewReadService)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _reviewReadService = reviewReadService;
        }

        public void DeleteReview(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", id);

            string query = "DELETE FROM reg_reviews WHERE review_id = @id;";

            _reviewRepository.DeleteReview(query, parameters);
        }

        public Review UpdateReview(ReviewUpdateInput reviewInput)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", reviewInput.Id);
            parameters.Add("@companyId", reviewInput.CompanyId);
            parameters.Add("@userId", reviewInput.UserId);
            parameters.Add("@stars", reviewInput.Stars);
            parameters.Add("@writtenReview", reviewInput.WrittenReview);
            parameters.Add("@anonymous", reviewInput.Anonymous);
            parameters.Add("@creationDate", reviewInput.CreationDate);
            parameters.Add("@verifiedReview", reviewInput.VerifiedReview);
            parameters.Add("@verifiedBy", reviewInput.VerifiedBy);

            string query = "UPDATE reg_reviews SET review_id = @id" +
                ", review_bedrijf_id = @companyId, review_student_id = @userId" +
                ", review_sterren = @stars, review_geschreven = @writtenReview" +
                ", review_anoniem = @anonymous, review_datum = @creationDate" +
                ", review_status = @verifiedReview, review_status_bevestigd_door = @verifiedBy" +
                " WHERE review_id = @id;";

            _reviewRepository.UpdateReview(query, parameters);

            return _reviewReadService.GetReviewById(reviewInput.Id);
        }
    }
}
