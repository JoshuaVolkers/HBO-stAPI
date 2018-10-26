using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.OptionModels;
using PoohAPI.Logic.Common.Models.PresentationModels;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("options")]
    public class OptionsController : Controller
    {
        private readonly IOptionReadService _optionReadService;

        public OptionsController(IOptionReadService reviewReadService)
        {
            _optionReadService = reviewReadService;
        }

        /// <summary>
        /// Gets the majors from which the students can choose
        /// </summary>
        /// <returns>A list of majors</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the request failed</response>
        /// <response code="404">If no majors were found</response>
        [HttpGet]
        [Route("majors")]
        [ProducesResponseType(typeof(IEnumerable<Major>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetMajors([FromQuery]int maxCount = 5, [FromQuery]int offset = 0)
        {
            if (maxCount < 1 || maxCount > 100)
                return BadRequest("MaxCount should be between 1 and 100");
            if (offset < 0)
                return BadRequest("Offset should be 0 or higher");

            IEnumerable<Major> majors = _optionReadService.GetAllMajors(maxCount, offset);

            if (majors is null)
                return NotFound("No majors found");

            return Ok(majors);
        }

        /// <summary>
        /// Gets the eduction levels
        /// </summary>
        /// <returns>A list of eduction levels</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the request failed</response>
        /// <response code="404">If no education levels were found</response>
        [HttpGet]
        [Route("eductionlevels")]
        [ProducesResponseType(typeof(IEnumerable<EducationLevel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetEducationLevels([FromQuery]int maxCount = 5, [FromQuery]int offset = 0)
        {
            if (maxCount < 1 || maxCount > 100)
                return BadRequest("MaxCount should be between 1 and 100");
            if (offset < 0)
                return BadRequest("Offset should be 0 or higher");

            IEnumerable<EducationLevel> educationLevels = _optionReadService.GetAllEducationLevels(maxCount, offset);

            if (educationLevels is null)
                return NotFound("No education levels found");

            return Ok(educationLevels);
        }       

        /// <summary>
        /// Gets the internship types to choose from
        /// </summary>
        /// <returns>A list of internships</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the request failed</response>
        /// <response code="404">If no internship types were found</response>

        [HttpGet]
        [Route("internshiptypes")]
        [ProducesResponseType(typeof(InternshipType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetInternshipTypes()
        {
            IEnumerable<InternshipType> internshipTypes = _optionReadService.GetAllInternshipTypes();

            if (internshipTypes is null)
                return NotFound("No internship types found");

            return Ok(internshipTypes);
        }

        /// <summary>
        /// Gets allowed email addresses
        /// </summary>
        /// <returns>A list of email addresses</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the request failed</response>
        /// <response code="404">If no email addresses were found</response>
        [HttpGet]
        [Route("allowedemailaddresses")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAllowedEmailAddresses([FromQuery]int maxCount = 5, [FromQuery]int offset = 0)
        {
            if (maxCount < 1 || maxCount > 100)
                return BadRequest("MaxCount should be between 1 and 100");
            if (offset < 0)
                return BadRequest("Offset should be 0 or higher");

            IEnumerable<AllowedEmailAddress> allowedEmails = _optionReadService.GetAllAllowedEmailAddresses(maxCount, offset);

            if (allowedEmails is null)
                return NotFound("No allowed emailaddresses found");

            return Ok(allowedEmails);
        }

        /// <summary>
        /// Gets languages for vacancies, companies and users
        /// </summary>
        /// <returns>A list of languages</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the request failed</response>
        /// <response code="404">If no languages were found</response>
        [HttpGet]
        [Route("languages")]
        [ProducesResponseType(typeof(IEnumerable<Language>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetLanguages()
        {
            IEnumerable<Language> languages = _optionReadService.GetAllLanguages();

            if (languages is null)
                return NotFound("No languages found");

            return Ok(languages);
        }
    }
}