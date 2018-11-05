using Microsoft.AspNetCore.Mvc;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.RequestModels;
using PoohAPI.Models.InputModels;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using PoohAPI.Authorization;
using System.Net;

namespace PoohAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("reviews")]
    public class ReviewsController : Controller
    {
        private readonly IReviewReadService _reviewReadService;
        private readonly IReviewCommandService _reviewCommandService;
        private readonly IHostingEnvironment _environment;

        public ReviewsController(IReviewReadService reviewReadService, IReviewCommandService reviewCommandService, IHostingEnvironment environment)
        {
            _reviewReadService = reviewReadService;
            _reviewCommandService = reviewCommandService;
            _environment = environment;
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
            var userId = CustomAuthorizationHelper.GetCurrentUserId(User);

            if (review == null)
            {
                return NotFound("Review not found.");
            }
            else if (review.UserId == userId)
            {
                return Ok(_reviewReadService.GetReviewById(id));
            }
            else return StatusCode((int)HttpStatusCode.Unauthorized, "User id does not match");
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
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult PostReview([FromBody]ReviewPost reviewData)
        {
            if (ModelState.IsValid)
            {
                return Ok(_reviewCommandService.PostReview(reviewData.CompanyId, CustomAuthorizationHelper.GetCurrentUserId(User), reviewData.Stars, reviewData.WrittenReview, reviewData.Anonymous));
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
        [Route("{Id}")]
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
                else if (CustomAuthorizationHelper.GetCurrentUserId(User) != reviewData.UserId)
                {
                    return StatusCode((int)HttpStatusCode.Unauthorized, "User id does not match");
                }
                else if (review.Locked == true)
                {
                    return BadRequest("Review has been locked.");
                }
<<<<<<< HEAD
                else return Ok(_reviewCommandService.UpdateReview(reviewData.Id, reviewData.CompanyId, CustomAuthorizationHelper.GetCurrentUserId(User), reviewData.Stars,
=======
                else return Ok(_reviewCommandService.UpdateReview(reviewData.Id, reviewData.CompanyId, review.UserId, reviewData.Stars,
>>>>>>> origin/Joshua
                        reviewData.WrittenReview, reviewData.Anonymous, reviewData.CreationDate,
                        reviewData.VerifiedReview, reviewData.VerifiedBy));
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
        /// <param name="id">The id of the review to delete</param>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the specified reviewId does not exist</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteReview(int id)
        {
            Review review = _reviewReadService.GetReviewById(id);
            if (review == null)
            {
                return BadRequest("Review not found.");
            }
            else if (CustomAuthorizationHelper.GetCurrentUserId(User) != review.UserId)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, "User id does not match");
            }
            else if (review.Locked == false)
            {
                _reviewCommandService.DeleteReview(id);
                return Ok("Review has been deleted.");
            }
            else
                return BadRequest("Review has been locked.");
        }
    }
}