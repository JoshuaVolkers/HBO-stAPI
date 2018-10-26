using AutoMapper;
using PoohAPI.Infrastructure.ReviewDB.Models;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.InputModels;
using System;
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

        public Review PostReview(int companyId, int userId, int stars, string writtenReview, int anonymous)
        {
            Review review = new Review();
            review.CompanyId = companyId;
            review.UserId = userId;
            review.Stars = stars;
            review.WrittenReview = writtenReview;
            review.Anonymous = anonymous;
            review.CreationDate = DateTime.Now;
            review.VerifiedReview = 0;
            review.VerifiedBy = 0;

            DBReview dbReview = _mapper.Map<DBReview>(review);

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@bedrijfId", dbReview.review_bedrijf_id);
            parameters.Add("@studentId", dbReview.review_student_id);
            parameters.Add("@sterren", dbReview.review_sterren);
            parameters.Add("@geschreven", dbReview.review_geschreven);
            parameters.Add("@anoniem", dbReview.review_anoniem);
            parameters.Add("@datum", dbReview.review_datum);
            parameters.Add("@status", dbReview.review_status);
            parameters.Add("@bevestigdDoor", dbReview.review_status_bevestigd_door);
            
            string query = "INSERT INTO reg_reviews (review_bedrijf_id, review_student_id" +
                ", review_sterren, review_geschreven, review_anoniem, review_datum" +
                ", review_status, review_status_bevestigd_door) " +
                "VALUES (@bedrijfId, @studentId, @sterren, @geschreven, @anoniem, @datum, @status, @bevestigdDoor);";

            _reviewRepository.PostReview(query, parameters);
            return _mapper.Map<Review>(dbReview);
        }
    }
}
