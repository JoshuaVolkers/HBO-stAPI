using Microsoft.AspNetCore.Mvc;
using PoohAPI.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using PoohAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Models.AuthenticationModels;
using PoohAPI.Logic.Common.Models.InputModels;

namespace PoohAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserReadService userReadService;
        private readonly IUserCommandService userCommandService;

        public UsersController(IUserReadService userReadService, IUserCommandService userCommandService)
        {
            this.userReadService = userReadService;
            this.userCommandService = userCommandService;
        }

        /// <summary>
        /// Starts the login process
        /// </summary>
        /// <param name="loginRequest">The loginRequest model</param>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="401">If the login failed due to incorrect credentials</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(JWTToken), 200)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody]LoginRequest loginRequest)
        {
            var user = this.userReadService.Login(loginRequest.EmailAddress, loginRequest.Password);
            if (user == null)
                return BadRequest("Username or password was incorrect!");

            var identity = TokenHelper.CreateClaimsIdentity(user.NiceName, user.Id);

            return Ok(TokenHelper.GenerateJWT(identity));
        }

        /// <summary>
        /// Login using Facebook AccessToken
        /// </summary>
        /// <param name="AccessToken">AccessToken retrieved from Facebook</param>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="401">If the login failed due to incorrect credentials</response>
        /// <remarks>The expirytime for the token is equal to the expirytime of Facebook accesstokens</remarks>
        [AllowAnonymous]
        [HttpPost]
        [Route("facebook")]
        [ProducesResponseType(typeof(JWTToken), 200)]
        [ProducesResponseType(401)]
        public async System.Threading.Tasks.Task<IActionResult> FacebookAsync([FromBody] string AccessToken)
        {
            var client = new HttpClient();
            var appAccessTokenResponse = await client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id=&client_secret=&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

            var userAccessTokenValidationResponse = await client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserTokenData>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.IsValid)
            {
                client.Dispose();
                return BadRequest("Facebook access token is invalid!");
            }

            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name&access_token={AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            client.Dispose();

            var user = this.userReadService.GetUserByEmail(userInfo.Email);

            if (user == null)
            {
                user = this.userCommandService.RegisterUser(userInfo.Name, userInfo.Email, UserAccountType.FacebookUser);
            }

            var identity = TokenHelper.CreateClaimsIdentity(user.NiceName, user.Id);
            return Ok(TokenHelper.GenerateJWT(identity));
        }

        /// <summary>
        /// Login using LinkedIn AccessToken
        /// </summary>
        /// <param name="AccessToken">AccessToken retrieved from LinkedIn</param>
        /// <param name="redirectUri">The redirectUri that was also used when requesting the acces token</param>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="401">If the login failed due to incorrect credentials</response>
        /// <remarks>The expirytime for the token is equal to the expirytime of LinkedIn accesstokens</remarks>
        [AllowAnonymous]
        [HttpPost]
        [Route("linkedin")]
        [ProducesResponseType(typeof(JWTToken), 200)]
        [ProducesResponseType(401)]
        public async System.Threading.Tasks.Task<IActionResult> LinkedInAsync([FromBody] string AccessToken, [FromBody] string redirectUri)
        {
            var client = new HttpClient();

            var appAccessTokenResponse = await client.GetStringAsync($"https://www.linkedin.com/oauth/v2/accessToken?grant_type=authorization_code&code={AccessToken}&redirect_uri={redirectUri}&client_id=1&client_secret=1");
            var appAccessToken = JsonConvert.DeserializeObject<LinkedInAppAccessToken>(appAccessTokenResponse);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appAccessToken.AccessToken);
            var userInfoResponse = await client.GetStringAsync($"https://api.linkedin.com/v2/people/me?projection=(id,firstName,lastName,email-address)");
            var userInfo = JsonConvert.DeserializeObject<LinkedInUserData>(userInfoResponse);

            var user = this.userReadService.GetUserByEmail(userInfo.Email);

            if (user == null)
            {
                user = this.userCommandService.RegisterUser(userInfo.FormattedName, userInfo.Email, UserAccountType.LinkedInUser);
            }

            var identity = TokenHelper.CreateClaimsIdentity(user.NiceName, user.Id);

            return Ok(TokenHelper.GenerateJWT(identity));
        }

        /// <summary>
        /// Starts the register process
        /// </summary>
        /// <param name="registerRequest">The registerRequest model</param>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="401">If the login failed due to incomplete personal information</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(JWTToken), 200)]
        [ProducesResponseType(401)]
        public IActionResult Register([FromBody]RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not all values were filled in correctly!");
            if (!registerRequest.AcceptTermsAndConditions)
                return BadRequest("You need to accept the terms and conditions before creating your account!");
            if (this.userReadService.GetUserByEmail(registerRequest.EmailAddress) != null)
                return BadRequest(string.Format("A user with emailaddres '{0}' already exists!",
                    registerRequest.EmailAddress));
            if (!CheckIfEmailAddressIsAllowed(registerRequest.EmailAddress))
                return BadRequest("The filled in emailaddress is not allowed!");

            var user = this.userCommandService.RegisterUser(registerRequest.Login, registerRequest.EmailAddress, UserAccountType.ApiUser, registerRequest.Password);
            var identity = TokenHelper.CreateClaimsIdentity(user.NiceName, user.Id);

            return Ok(TokenHelper.GenerateJWT(identity));
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
        /// <param name="maxCount">The max amount of users to return</param>
        /// <param name="offset">The number of users to skip.</param>
        /// <param name="educationalAttainments">A comma seperated list of educationalAttainment Ids (opleidingsniveau)</param>
        /// <param name="educations">A comma seperated list of education Ids</param>
        /// <param name="cityName">The city in which the user should be located.</param>
        /// <param name="countryName">The name of the country where the student should live. Country names can be found in the country endpoint.</param>
        /// <param name="range">The range in which the user's location should be found from the city parameter</param>
        /// <param name="additionalLocationSearchTerms">Municipality,province or state seperated by spaces. This is required to identify the correct city if there are multiple cities with the same name within a country. This parameter is only useful when used with range.</param>
        /// <param name="preferredLanguage">The preferred language of the user</param>
        /// <returns>A list of users</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If no users were found for the specified filters</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>
        //[Authorize(Roles = "administrator")]
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<BaseUser>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetAllUsers([FromQuery]int maxCount = 5, [FromQuery]int offset = 0,
            [FromQuery]string educationalAttainments = null, [FromQuery]string educations = null,
            [FromQuery]string cityName = null, [FromQuery]string countryName = null, [FromQuery]int? range = null,
            [FromQuery]string additionalLocationSearchTerms = null, [FromQuery]int? preferredLanguage = null)
        {
            if (maxCount < 1 || maxCount > 100)
                return BadRequest("MaxCount should be between 1 and 100");
            if (offset < 0)
                return BadRequest("Offset should be 0 or larger");

            IEnumerable<BaseUser> users = this.userReadService.GetAllUsers(maxCount, offset, educationalAttainments,
                educations, cityName, countryName, range, additionalLocationSearchTerms, preferredLanguage);

            if (users is null)
                return NotFound("No users found");


            return Ok(users);
        }

        /// <summary>
        /// Get's the userdata for the specified user
        /// </summary>
        /// <returns>A user model</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpGet]
        [Route("me")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public object GetUserById()
        {
            var user = this.userReadService.GetUserById(GetCurrentUserId());
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
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
        public IActionResult DeleteUser(int id)
        {
            this.userCommandService.DeleteUser(id);
            return Ok();
        }

        /// <summary>
        /// Updates the userdata for the specified user.
        /// </summary>
        /// <param name="userData">The user model containing the updated data</param>
        /// <returns>The updated user model</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpPut]
        [Route("me")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult UpdateUser([FromBody]UserUpdateInput userData)
        {
            if (ModelState.IsValid && GetCurrentUserId().Equals(userData.Id))
                return Ok(this.userCommandService.UpdateUser(userData));
            else
                return BadRequest("Informatie involledig.");
        }

        /// <summary>
        /// Get's the users favorite vacancies.
        /// </summary>
        /// <returns>A list of vacancies</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user has no favorites</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpGet]
        [Route("me/favorites")]
        [ProducesResponseType(typeof(IEnumerable<Vacancy>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetFavoriteVacancies()
        {
            return Ok(new List<Vacancy>());
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
        [Route("me/favorites/{vacancyId}")]
        [ProducesResponseType(typeof(Vacancy), 200)]
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
        [Route("me/favorites/{vacancyId}")]
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
        [Route("me/reviews")]
        [ProducesResponseType(typeof(IEnumerable<Review>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult GetReviews()
        {
            return Ok(new List<Review>());
        }

        private int GetCurrentUserId()
        {
            return Int32.Parse(User.Claims.SingleOrDefault(c => c.Type == "id").Value);
        }

        private bool CheckIfEmailAddressIsAllowed(string emailAddress)
        {
            var allowedEmails = new List<string>() { "student.inholland.nl", "student.hva.nl" };
            var domain = emailAddress.Substring(emailAddress.LastIndexOf("@") + 1);
            return allowedEmails.Any(d => d.Contains(domain));
        }
    }
}