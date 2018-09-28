using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.RequestModels;
using System;
using System.Collections.Generic;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserReadService _userReadService;

        public UsersController(IUserReadService userReadService)
        {
            _userReadService = userReadService;
        }

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
        /// Checks if token is still valid
        /// </summary>
        /// <param name="token">The token to be checked</param>
        /// <returns>A boolean that says if the token if valid</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If no token was given as parameter</response>
        [HttpPost]
        [Route("checktoken")]
        [ProducesResponseType(typeof(Boolean), 200)]
        [ProducesResponseType(400)]
        public IActionResult CheckToken([FromBody]Token token)
        {
            return Ok(new Boolean());
        }

        /// <summary>
        /// Get's all of the users. (Admin rights are required for this endpoint!)
        /// </summary>
        /// <param name="educationalAttainment">A comma seperated list of educationalAttainments (opleidingsniveau)</param>
        /// <param name="educations">A comma seperated list of educations</param>
        /// <param name="city">The city in which the user should be located.</param>
        /// <param name="range">The range in which the user's location should be found from the city parameter</param>
        /// <param name="preferredLanguages">A comma seperated list of preferredLanguages of which the user should have set at least one</param>
        /// <returns>A list of users</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If no users were found for the specified filters</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetAllUsers([FromQuery]string educationalAttainment = null, [FromQuery]string educations = null,
            [FromQuery]string city = null, [FromQuery]double range = 5.0, [FromQuery]string preferredLanguages = null)
        {
            return Ok(_userReadService.GetAllUsersAsync().Result);
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
            return Ok(_userReadService.GetUserByIdAsync(id).Result);
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
        [Route("{id}/favorites")]
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

    }
}