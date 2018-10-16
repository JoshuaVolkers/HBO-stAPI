using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("vacancies")]
    public class VacanciesController : Controller
    {
        private readonly IVacancyReadService vacancyReadService;
        private readonly IVacancyCommandService vacancyCommandService;

        public VacanciesController(IVacancyReadService vacancyReadService, IVacancyCommandService vacancyCommandService)
        {
            this.vacancyReadService = vacancyReadService;
            this.vacancyCommandService = vacancyCommandService;
        }

        /// <summary>
        /// Gets a list of all vacancies
        /// </summary>
        /// <remarks>Returns Vacancies or BaseVacancies. The model used is determined by detailedVacancy.</remarks>
        /// <param name="maxCount">The max amount of vacancies to return, defaults to 5</param>
        /// <param name="offset">The number of vacancies to skip</param>
        /// <param name="additionalLocationSearchTerms">Searchwords to narrow the resultsets, comma seperated list</param>
        /// <param name="education">The name of the education</param>
        /// <param name="educationalAttainment">The level of the education (HBO, WO, Univerity, etc.)</param>
        /// <param name="intershipType">The type of intership</param>
        /// <param name="languages">A comma seperated list of the languages to get vacancies for</param>
        /// <param name="cityname">The city name where the vacancy is located in</param>
        /// <param name="countryname">The coutry name where the vacancy is located in</param>
        /// <param name="locationrange">The range where the vacancies must be retrieved within</param>
        /// <returns>A list of all vacancies</returns>
        /// <response code="200">Returns the list of vacancies</response>
        /// <response code="404">If no vacancies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vacancy>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5, [FromQuery]int offset = 0, [FromQuery]string additionalLocationSearchTerms = null, [FromQuery]string education = null, [FromQuery]string educationalAttainment = null, [FromQuery]IntershipType? intershipType = null, [FromQuery]string languages = null, [FromQuery]string cityname = null, [FromQuery]string countryname = null, [FromQuery]int? locationrange = null)
        {
            if (maxCount < 0 || maxCount > 100)
            {
                return BadRequest("MaxCount should be between 1 and 100");
            }

            if (offset < 0)
            {
                return BadRequest("Offset should be 0 or larger");
            }

            IEnumerable<Vacancy> vacancies = this.vacancyReadService.GetListVacancies(maxCount, offset, additionalLocationSearchTerms, education, educationalAttainment, intershipType, languages, cityname, countryname, locationrange);

            if (!(vacancies is null))
            {
                return Ok(vacancies);
            }
            else
            {
                return NotFound("No vacancies were found");
            }
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