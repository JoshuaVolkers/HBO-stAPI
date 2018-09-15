using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Models;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/companies")]
    public class CompaniesController : Controller
    {
        /// <summary>
        /// Gets a list of all companies
        /// </summary>
        /// <param name="maxCount">The max amount of companies to return, defaults to 5</param>
        /// <param name="offset">The number of companies to skip</param>
        /// <param name="minStars">The min number of stars that the returning companies should have</param>
        /// <param name="maxStars">The max number of stars that the returning companies should have</param>
        /// <param name="cityName">The city in which the companies should be located</param>
        /// <param name="countryCode">The ISO alpha-2 country code</param>
        /// <param name="tag">The tag which the returning companies should have</param>
        /// <returns>A list of all companies</returns>
        /// <response code="200">Returns the list of companies</response>
        /// <response code="404">If no companies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Company>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5, [FromQuery]int offset = 0, [FromQuery]double minStars = 0, 
            [FromQuery]double maxStars = 5, [FromQuery]string cityName = null, [FromQuery]string countryCode = null,
            [FromQuery]string tag = null)
        {
            return Ok(new List<Company>());
        }

        /// <summary>
        /// Retrieves a company by its id
        /// </summary>
        /// <param name="id">The id of a company</param>
        /// <returns>A specific company</returns>
        /// <response code="200">Returns the requested company</response>
        /// <response code="404">If the specified company was not found</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Company), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            return Ok(new Company() { Id = id });
        }

        /// <summary>
        /// Retrieves anonymous reviews for a company by its id
        /// </summary>
        /// <param name="id">The id of a company</param>
        /// <returns>A list of anonymous reviews</returns>
        /// <response code="200">Returns the anonymous reviews of the company</response>
        /// <response code="404">If the specified company was not found</response>   
        [HttpGet("{id}/reviews")]
        [ProducesResponseType(typeof(IEnumerable<ReviewAnonymous>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetCompanyReviews(int id)
        {
            return Ok(new List<ReviewAnonymous>());
        }
    }
}
