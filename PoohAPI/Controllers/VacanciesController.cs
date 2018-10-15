using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("vacancies")]
    public class VacanciesController : Controller
    {
        /// <summary>
        /// Gets a list of all vacancies
        /// </summary>
        /// <remarks>Returns Vacancies or BaseVacancies. The model used is determined by detailedVacancy.</remarks>
        /// <param name="maxCount">The max amount of vacancies to return, defaults to 5</param>
        /// <param name="offset">The number of vacancies to skip</param>
        /// <param name="searchWords">Searchwords to narrow the resultsets, comma seperated list</param>
        /// <param name="distanceInKM">Distance to the company's location, calculated from the location in the user's profile</param>
        /// <param name="education">The name of the education</param>
        /// <param name="educationalAttainment">The level of the education (HBO, WO, Univerity, etc.)</param>
        /// <param name="intershipType">The type of intership</param>
        /// <param name="languages">A comma seperated list of the languages to get vacancies for</param>
        /// <returns>A list of all vacancies</returns>
        /// <response code="200">Returns the list of vacancies</response>
        /// <response code="404">If no vacancies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vacancy>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5, [FromQuery]int offset = 0, [FromQuery]string searchWords = null, [FromQuery]double distanceInKM = 10.0,
            [FromQuery]string education = null, [FromQuery]string educationalAttainment = null, [FromQuery]IntershipType? intershipType = null, [FromQuery]string languages = null)
        {
            return Ok(new List<Vacancy>());
        }

        /// <summary>
        /// Gets a specific vacancy by Id
        /// </summary>
        /// <param name="id">The Id of the vacancy to retrieve</param>
        /// <returns>One specific vacancy</returns>
        /// <response code="200">Returns the requested vacancy</response>
        /// <response code="404">If the specified vacancy was not found</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Vacancy), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            return Ok(new Vacancy() { Id = id });
        }
    }
}