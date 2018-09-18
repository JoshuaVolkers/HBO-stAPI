using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Models;
using PoohAPI.Models.BaseModels;
using PoohAPI.Models.RequestModels;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UsersController : Controller
    {

        /// <summary>
        /// Starts the login process
        /// </summary>
        /// <param name="loginRequest">The loginRequest model</param>
        /// <returns>The logged in user</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="401">If the login failed due to incorrect credentials</response>  
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody]LoginRequest loginRequest)
        {
            return Ok();
        }
        
        /// <summary>
        /// Starts the register process
        /// </summary>
        /// <param name="registerRequest">The registerRequest model</param>
        /// <returns>The registered user</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="401">If the login failed due to incomplete personal information</response>  
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        public IActionResult Register([FromBody]RegisterRequest registerRequest)
        {
            return Ok();
        }

        /// <summary>
        /// Get's all of the users. (Admin rights are required for this endpoint!)
        /// </summary>
        /// <param name="educationalAttainment">A comma seperated list of educationalAttainments (opleidingsniveau)</param>
        /// <param name="educations">A comma seperated list of educations</param>
        /// <param name="location">A Location model, used to determine the range in which the user should have his location set</param>
        /// <param name="preferredLanguages">A comma seperated list of preferredLanguages of which the user should have set at least one</param>
        /// <returns>A list of users</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If no users were found for the specified filters</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetAllUsers([FromQuery]string educationalAttainment = null, [FromQuery]string educations = null,
            [FromQuery]Location location = null, [FromQuery]string preferredLanguages = null)
        {
            return Ok(new List<User>());
        }

        /// <summary>
        /// Get's the userdata for the specified user
        /// </summary>
        /// <param name="id">The id of the user to retrieve</param>
        /// <returns>A user model</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetUserById(int id)
        {
            return Ok(new User() { Id = id });
        }

        /// <summary>
        /// Deletes the entire user, this includes the login credentials.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response> 
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        /// <response code="404">If the user was not found</response>  
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser()
        {
                return Ok();
        }

        /// <summary>
        /// Updates the userdata for the specified user.
        /// </summary>
        /// <param name="id">The id of the user to update</param>
        /// <param name="userData">The user model containing the updated data</param>
        /// <returns>The updated user model</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult UpdateUser(int id, [FromBody]User userData)
        {
            return Ok(userData);
        }

        /// <summary>
        /// Get's the users favorite vacancies.
        /// </summary>
        /// <param name="id">The id of the user whose favorite vacancies should be retrieved</param>
        /// <returns>A list of vacancies</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user has no favorites</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpGet]
        [Route("{id}/vacancies")]
        [ProducesResponseType(typeof(IEnumerable<BaseVacancy>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetFavoriteVacancies(int id)
        {
            return Ok(new List<BaseVacancy>());
        }

        /// <summary>
        /// Adds the specified vacancy to the favorites list of the user
        /// </summary>
        /// <param name="vacancyId">The id of the vacancy to add to the favorites list</param>
        /// <returns>The liked vacancy as a BaseVacancy model</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified vacancy was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpPut]
        [Route("{id}/favorites/{vacancyId}")]
        [ProducesResponseType(typeof(BaseVacancy), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult AddVacancyToFavorites(int vacancyId = 1)
        {
            //Retrieve baseVacancy for ID, if found continue, else badRequest.
            if (vacancyId == 1)
                return Ok();
            else
                return BadRequest("No vacancy found with ID: " + vacancyId);
        }

        /// <summary>
        /// Removes the specified vacancy from the favorites list of the user
        /// </summary>
        /// <param name="vacancyId">The id of the vacancy to add to the favorites list</param>
        /// <returns>The liked vacancy as a BaseVacancy model</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the specified vacancy was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpDelete]
        [Route("{id}/favorites/{vacancyId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult RemoveVacancyFromFavorites(int vacancyId = 1)
        {
            //Retrieve baseVacancy for ID, if found continue, else badRequest.
            if (vacancyId == 1)
                return Ok();
            else
                return BadRequest("No vacancy found with ID: " + vacancyId);
        }

        /// <summary>
        /// Get's the users reviews.
        /// </summary>
        /// <returns>A list of Review objects</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the user has no reviews</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpGet]
        [Route("{id}/reviews")]
        [ProducesResponseType(typeof(IEnumerable<Review>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetReviews()
        {
            return Ok(new List<Review>());
        }

        /// <summary>
        /// Updates the specified review.
        /// </summary>
        /// <remarks>Reviews can only be updated until 72 hours after they have been created. Otherwise they will be locked.</remarks>
        /// <param name="reviewId">The id of the review to update</param>
        /// <param name="reviewData">The updated review model</param>
        /// <returns>A list of Review objects</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the specified review does not exist</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpPut]
        [Route("{id}/reviews/{reviewId}")]
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult UpdateReview(int reviewId, [FromBody]Review reviewData)
        {
            //Check of review niet locked is.
            if (reviewId == 1)
                return Ok(new Review() { Id = reviewId });
            else
                return BadRequest("Review has been locked");
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
        [Route("{id}/reviews/{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            //Check of review niet locked is.
            if (reviewId == 1)
                return Ok();
            else
                return BadRequest("Review has been locked");
        }

    }
}