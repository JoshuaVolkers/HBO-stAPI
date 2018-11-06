using Microsoft.AspNetCore.Mvc;
using PoohAPI.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PoohAPI.Authorization;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Models.AuthenticationModels;
using PoohAPI.Models.InputModels;

namespace PoohAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserReadService userReadService;
        private readonly IUserCommandService userCommandService;
        private readonly IVacancyReadService vacancyReadService;
        private readonly IVacancyCommandService vacancyCommandService;
        private readonly ITokenHelper tokenHelper;
        private readonly IOptionReadService optionReadService;

        public UsersController(IUserReadService userReadService, IUserCommandService userCommandService, IVacancyCommandService vacancyCommandService, IVacancyReadService vacancyReadService, ITokenHelper tokenHelper, IOptionReadService optionReadService)
        {
            this.userReadService = userReadService;
            this.userCommandService = userCommandService;
            this.vacancyReadService = vacancyReadService;
            this.vacancyCommandService = vacancyCommandService;
            this.tokenHelper = tokenHelper;
            this.optionReadService = optionReadService;
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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        public IActionResult Login([FromBody]LoginRequest loginRequest)
        {
            var user = this.userReadService.Login(loginRequest.Password, loginRequest.EmailAddress);
            if (user == null)
                return StatusCode((int)HttpStatusCode.Unauthorized, "Username or password was incorrect!");

            var refreshToken = this.userCommandService.UpdateRefreshToken(user.Id);

            var identity = this.tokenHelper.CreateClaimsIdentity(user.Active, user.Id, user.Role.ToString(), refreshToken);

            return Ok(this.tokenHelper.GenerateJWT(identity));
        }

        /// <summary>
        /// Login using Facebook AccessToken
        /// </summary>
        /// <param name="AccessToken">AccessToken retrieved from Facebook</param>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="401">If the login failed due to incorrect credentials</response>
        /// <remarks>The expirytime for the token is equal to the expirytime of Facebook accesstokens</remarks>
        [Obsolete]
        [AllowAnonymous]
        [HttpPost]
        [Route("facebook")]
        [ProducesResponseType(typeof(string), 200)]
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
                return StatusCode((int)HttpStatusCode.Unauthorized, "Facebook access token is invalid!");
            }

            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name&access_token={AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            client.Dispose();

            var user = this.userReadService.GetUserByEmail<JwtUser>(userInfo.Email);

            if (user == null)
            {
                user = this.userCommandService.RegisterUser(userInfo.Name, userInfo.Email, UserAccountType.FacebookUser);
            }

            var identity = this.tokenHelper.CreateClaimsIdentity(user.Active, user.Id, user.Role.ToString(), user.RefreshToken);
            return Ok(this.tokenHelper.GenerateJWT(identity));
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
        [Obsolete]
        [AllowAnonymous]
        [HttpPost]
        [Route("linkedin")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        public async System.Threading.Tasks.Task<IActionResult> LinkedInAsync([FromBody] string AccessToken, [FromBody] string redirectUri)
        {
            var client = new HttpClient();

            var appAccessTokenResponse = await client.GetStringAsync($"https://www.linkedin.com/oauth/v2/accessToken?grant_type=authorization_code&code={AccessToken}&redirect_uri={redirectUri}&client_id=1&client_secret=1");
            var appAccessToken = JsonConvert.DeserializeObject<LinkedInAppAccessToken>(appAccessTokenResponse);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", appAccessToken.AccessToken);
            var userInfoResponse = await client.GetStringAsync($"https://api.linkedin.com/v2/people/me?projection=(id,firstName,lastName,email-address)");
            var userInfo = JsonConvert.DeserializeObject<LinkedInUserData>(userInfoResponse);

            var user = this.userReadService.GetUserByEmail<JwtUser>(userInfo.Email);

            if (user == null)
            {
                user = this.userCommandService.RegisterUser(userInfo.FormattedName, userInfo.Email, UserAccountType.LinkedInUser);
            }

            var identity = this.tokenHelper.CreateClaimsIdentity(user.Active, user.Id, user.Role.ToString(), user.RefreshToken);

            return Ok(this.tokenHelper.GenerateJWT(identity));
        }

        /// <summary>
        /// Starts the register process
        /// </summary>
        /// <param name="registerRequest">The registerRequest model</param>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200">If the request was a success</response>  
        /// <response code="400">If the login failed due to incomplete personal information</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public IActionResult Register([FromBody]RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not all values were filled in correctly!");

            if (!registerRequest.AcceptTermsAndConditions)
                return BadRequest("You need to accept the terms and conditions before creating your account!");

            if (!CheckIfEmailAddressIsAllowed(registerRequest.EmailAddress))
                return BadRequest("The filled in emailaddress is not allowed!");

            User existingUser = this.userReadService.GetUserByEmail<User>(registerRequest.EmailAddress);
            if (existingUser != null)
            {
                if (existingUser.Active)
                    return BadRequest(string.Format("A user with emailaddres '{0}' already exists!", 
                        registerRequest.EmailAddress));

                // Just in case the user exists but did not verify his email in time
                this.userCommandService.CreateUserVerification(existingUser.Id);
                return Ok("Verification email has been sent.");
            }                     

            var user = this.userCommandService.RegisterUser(registerRequest.Login, registerRequest.EmailAddress, UserAccountType.ApiUser, registerRequest.Password);

            return Ok("Verification email has been sent.");
        }

        /// <summary>
        /// Request a new acces token by using the refreshtoken.
        /// </summary>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="200">If the refreshToken is not a valid guid</response>
        /// <response code="404">If the refreshtoken does not exist</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("token/{refreshToken}/refresh")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult RefreshAccesToken(string refreshToken)
        {
            if (Guid.TryParse(refreshToken, out Guid parsedGuid))
            {
                var user = this.userReadService.GetUserByRefreshToken(parsedGuid.ToString("N"));
                if (user == null)
                    return NotFound("Specified token does not exist!");

                var identity = this.tokenHelper.CreateClaimsIdentity(user.Active, user.Id, user.Role.ToString(), parsedGuid.ToString("N"));

                return Ok(this.tokenHelper.GenerateJWT(identity));
            }

            return BadRequest("Refresh token is not a valid GUID!");
        }

        /// <summary>
        /// Revokes the refresh token, invalidating it for future use.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the refreshtoken does not exist</response>
        [AllowAnonymous]
        [HttpDelete]
        [Route("token/{refreshToken}/revoke")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult RevokeRefreshToken(string refreshToken)
        {
            if (Guid.TryParse(refreshToken, out Guid parsedGuid))
            {
                this.userCommandService.DeleteRefreshToken(parsedGuid.ToString("N"));
                return Ok();
            }
            return BadRequest("Refresh token is not a valid GUID!");
        }

        /// <summary>
        /// Verifies email address of newly registered users and activates their accounts.
        /// </summary>
        /// <param name="token">Token that is send to the users' email address.</param>
        /// <returns>A JWTtoken used when accessing protected endpoints</returns>
        /// <response code="200"></response>
        /// <response code="400"></response>
        [AllowAnonymous]
        [HttpGet]
        [Route("verify")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public IActionResult VerifyEmail([FromQuery]string token)
        {
            User user = this.userCommandService.VerifyUserEmail(token);

            if (user is null)
                return BadRequest("Token is invalid or expired.");

            return Ok("Your email address has been verified. Please, log into your account with your application.");
        }

        /// <summary>
        /// Get's all of the users. ('Validator' or 'Elbho_medewerker' role is required for this endpoint!)
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
        /// <response code="400">If the request was invalid</response>
        [Authorize(Roles = "Validator, Elbho_medewerker")]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public IActionResult GetAllUsers([FromQuery]int maxCount = 5, [FromQuery]int offset = 0,
            [FromQuery]string educationalAttainments = null, [FromQuery]string educations = null,
            [FromQuery]string cityName = null, [FromQuery]string countryName = null, [FromQuery]int? range = null,
            [FromQuery]string additionalLocationSearchTerms = null, [FromQuery]int? preferredLanguage = null)
        {
            if (maxCount < 1 || maxCount > 100)
                return BadRequest("MaxCount should be between 1 and 100");
            if (offset < 0)
                return BadRequest("Offset should be 0 or larger");

            IEnumerable<User> users = this.userReadService.GetAllUsers(maxCount, offset, educationalAttainments,
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
        public IActionResult GetUserById()
        {
            var user = this.userReadService.GetUserById(CustomAuthorizationHelper.GetCurrentUserId(User), false);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        /// <summary>
        /// Deletes the entire user, this includes the login credentials.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response> 
        /// <response code="404">If the user was not found</response>
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>  
        [HttpDelete]
        [Route("me")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult DeleteUser()
        {
            if (this.userReadService.GetUserById(CustomAuthorizationHelper.GetCurrentUserId(User), false) == null)
                return NotFound("User not found.");

            this.userCommandService.DeleteUser(CustomAuthorizationHelper.GetCurrentUserId(User));
            return Ok("User deleted.");
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
        /// <response code="401">If the request was invalid</response>
        [HttpPut]
        [Route("me")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public IActionResult UpdateUser([FromBody]UserUpdateInput userData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Body incorrect.");
            
            if (this.userReadService.GetUserById(CustomAuthorizationHelper.GetCurrentUserId(User), false) == null)
                return NotFound("User not found.");

            return Ok(this.userCommandService.UpdateUser(userData.CountryId, userData.City, userData.EducationId, userData.EducationalAttainmentId, userData.PreferredLanguageId, CustomAuthorizationHelper.GetCurrentUserId(User), userData.AdditionalLocationIdentifier));
        }

        /// <summary>
        /// Updates the password for the authenticated user.
        /// </summary>
        /// <param name="passwordUpdateInput">The password model containing the new and old password</param>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>
        [HttpPut]
        [Route("me/updatepassword")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult UpdatePassword([FromBody]PasswordUpdateInput passwordUpdateInput)
        {
            if (ModelState.IsValid)
            {
                if(userCommandService.UpdatePassword(CustomAuthorizationHelper.GetCurrentUserId(User), passwordUpdateInput.NewPassword, passwordUpdateInput.OldPassword))
                {
                    return Ok("Password has been changed.");
                }
                else
                {
                    return BadRequest("Old password not correct!");
                }
            }

            else
            {
                return BadRequest("Not all fields were filled in correctly");
            }
        }

        /// <summary>
        /// Sends an email to the given email to reset the password
        /// </summary>
        /// <param name="emailAddress">The email to reset the password for and send the mail to</param>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified user was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("forgotpassword")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult ForgotPassword(string emailAddress)
        {
            User user = this.userReadService.GetUserByEmail<User>(emailAddress);
            if (user is null)
            {
                return BadRequest("User with given email does not exist!");
            }

            else
            {
                userCommandService.ResetPassword(emailAddress, user.Id);
                return Ok("Reset email has been sent to the email!");
            }
        }

        /// <summary>
        /// Generates a new password for the user.
        /// </summary>
        /// <param name="token">The token generated and sent to the email to verify the reset</param>
        /// <returns></returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the token was invalid</response>   
        /// <response code="403">If the user was unauthorized</response>  
        /// <response code="401">If the user was unauthenticated</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("verifyreset")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        public IActionResult VerifyReset(string token)
        {
            string newpassword = this.userCommandService.VerifyResetPassword(token);

            if (newpassword is null)
                return BadRequest("Token is invalid or expired.");

            return Ok("Your password has been reset, your new password is: " + newpassword);
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
            //Get list of favorite vacancies with the user id of the authenticated user
            IEnumerable<Vacancy> vacancies = vacancyReadService.GetFavoriteVacancies(CustomAuthorizationHelper.GetCurrentUserId(User));

            //If there are results
            if (!(vacancies is null))
            {
                return Ok(vacancies);
            }

            //If no results were found
            else
            {
                return NotFound("No vacancies were found");
            }
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
        public IActionResult AddVacancyToFavorites(int vacancyId)
        {
            //Check if the vacancy exists
            Vacancy vacancy = vacancyReadService.GetVacancyById(vacancyId);

            //If it exists
            if(vacancy != null)
            {
                //Get authenticated user id
                int userid = CustomAuthorizationHelper.GetCurrentUserId(User);
                vacancyCommandService.AddFavourite(userid, vacancyId);
                return Ok(String.Format("Vacancy with id {0} has been added to favorite of user with user id {1}", vacancyId, userid));
            }

            //If not
            else
            {
                return NotFound("Specified vacancy was not found!");
            }
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
        public IActionResult RemoveVacancyFromFavorites(int vacancyId)
        {
            //Retrieve list of vacancies that are favorited by the authenticated user
            IEnumerable<Vacancy> vacancies = vacancyReadService.GetFavoriteVacancies(CustomAuthorizationHelper.GetCurrentUserId(User));

            //If the vacancy that is being removed exists as a favorite
            if (vacancies.Any(c => c.Id == vacancyId))
            {
                int userid = CustomAuthorizationHelper.GetCurrentUserId(User);
                vacancyCommandService.DeleteFavourite(userid, vacancyId);
                return Ok(String.Format("Vacancy has been deleted from favorites of user with user id {0}", userid));
            }

            //If not
            else
            {
                return NotFound("Specified vacancy was not found!");
            }
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

        private bool CheckIfEmailAddressIsAllowed(string emailAddress)
        {
            var allowedEmails = this.optionReadService.GetAllAllowedEmailAddresses(0, 0)
                .Select(x => x.EmailAddress);
            var domain = emailAddress.Substring(emailAddress.LastIndexOf("@") + 1);
            return allowedEmails.Any(address => address.Contains(domain));
        }
    }
}