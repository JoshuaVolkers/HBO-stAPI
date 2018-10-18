using Microsoft.AspNetCore.Mvc;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.RequestModels;
using PoohAPI.Logic.Common.Models.InputModels;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("reviews")]
    public class ReviewsController : Controller
    {
        private readonly IReviewReadService _reviewReadService;
        private readonly IReviewCommandService _reviewCommandService;

        public ReviewsController(IReviewReadService reviewReadService, IReviewCommandService reviewCommandService)
        {
            _reviewReadService = reviewReadService;
            _reviewCommandService = reviewCommandService;
        }

        /// <summary>
        /// Gets review by id for editing purposes. The user must be authorized.
        /// </summary>
        /// <remarks>Reviews can only be updated until 72 hours after they have been created. Otherwise they will be locked.</remarks>
        /// <param name="id">The id of the review</param>
        /// <returns>A review model</returns>
        /// <response code="200">Returns the specified review.</response>
        /// <response code="400">If the required fields were not included</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response> 
        /// <response code="404">If the review was not found</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public IActionResult GetReviewById(int id)
        {
            Review review = _reviewReadService.GetReviewById(id);

            if (review == null)
            {
                return NotFound("Review not found.");
            }
            else
            {
                return Ok(_reviewReadService.GetReviewById(id));
            }
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <remarks>Reviews can only be updated until 72 hours after they have been created. Otherwise they will be locked.</remarks>
        /// <param name="reviewData">The updated review model</param>
        /// <returns>A list of Review objects</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the required fields were not included</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(ReviewPost), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult PostReview([FromBody]ReviewPost reviewData)
        {
            if (ModelState.IsValid)
            {
                return Ok(_reviewCommandService.PostReview(reviewData.CompanyId, reviewData.UserId, reviewData.Stars, reviewData.WrittenReview, reviewData.Anonymous));                
            }
            else
            {
                return BadRequest("Review information is incomplete");
            }
        }

        /// <summary>
        /// Updates the specified review.
        /// </summary>
        /// <remarks>Reviews can only be updated until 72 hours after they have been created. Otherwise they will be locked.</remarks>
        /// <param name="reviewData">The updated review model</param>
        /// <returns>A list of Review objects</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the specified review does not exist</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpPut]
        [Route("{reviewId}")]
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult UpdateReview([FromBody]ReviewUpdateInput reviewData)
        {
            if (ModelState.IsValid)
            {
                Review review = _reviewReadService.GetReviewById(reviewData.Id);
                if (review == null)
                {
                    return BadRequest("Review not found.");
                }
                else if (review.Locked == true)
                {
                    return BadRequest("Review has been locked.");                    
                }
                else
                    return Ok(_reviewCommandService.UpdateReview(reviewData));
            }
            else
            {
                return BadRequest("Review information is incomplete");
            }            
        }

        /// <summary>
        /// Deletes the specified review
        /// </summary>
        /// <remarks>Reviews can only be deleted until 72 hours after they have been created. Otherwise they will be locked.</remarks>
        /// <param name="reviewId">The id of the review to delete</param>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the specified reviewId does not exist</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpDelete]
        [Route("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            Review review = _reviewReadService.GetReviewById(reviewId);
            if(review == null)
            {
                return BadRequest("Review not found.");
            }
            else if (review.Locked == false)
            {
                _reviewCommandService.DeleteReview(reviewId);
                return Ok("Review has been deleted.");
            }                
            else
                return BadRequest("Review has been locked.");
        }
    }
}