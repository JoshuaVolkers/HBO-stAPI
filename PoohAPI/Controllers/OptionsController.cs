using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Models;
using PoohAPI.Models.BaseModels;
using PoohAPI.Models.RequestModels;
using PoohAPI.Models.OptionModels;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("options")]
    public class OptionsController : Controller
    {
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
        public IActionResult GetMajors()
        {
            return Ok(new List<Major>());
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
        public IActionResult GetEducationLevels()
        {
            return Ok(new List<EducationLevel>());
        }

        /// <summary>
        /// Gets the cities which are registered within the jobs module of hbo-stagemarkt
        /// </summary>
        /// <param name="countryId">The id of the country in which the city is located</param>
        /// <returns>A list of cities</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the request failed</response>
        /// <response code="404">If no cities were found</response>
        [HttpGet]
        [Route("cities")]
        [ProducesResponseType(typeof(IEnumerable<City>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCities([FromQuery]int countryId)
        {
            return Ok(new List<City>());
        }

        /// <summary>
        /// Gets the countries which are registered within the jobs module of hbo-stagemarkt
        /// </summary>
        /// <returns>A list of countries</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="400">If the request failed</response>
        /// <response code="404">If no cities were found</response>
        [HttpGet]
        [Route("countries")]
        [ProducesResponseType(typeof(IEnumerable<Country>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCountries()
        {
            return Ok(new List<Country>());
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
        [ProducesResponseType(typeof(IntershipType), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetInternshipTypes()
        {
            return Ok(new IntershipType());
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
        public IActionResult GetAllowedEmailAddresses()
        {
            return Ok(new List<string>());
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
            return Ok(new List<Language>());
        }
    }
}