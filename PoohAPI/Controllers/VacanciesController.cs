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
    [Route("api/vacancies")]
    public class VacanciesController : Controller
    {
        /// <summary>
        /// Gets a list of all vacancies
        /// </summary>
        /// <param name="maxCount">The max amount of vacancies to return, defaults to 5</param>
        /// <returns>A list of all vacancies</returns>
        /// <response code="200">Returns the list of vacancies</response>
        /// <response code="404">If no vacancies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vacancy>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5)
        {
            return Ok(new List<Vacancy>());
        }

        /// <summary>
        /// Gets a list of all basic vacancies
        /// </summary>
        /// <returns>A list of all basic vacancies</returns>
        /// <param name="maxCount">The max amount of vacancies to return, defaults to 5</param>
        /// <response code="200">Returns the list of basic vacancies</response>
        /// <response code="404">If no basic vacancies are found</response>   
        [HttpGet]
        [Route("basic")]
        [ProducesResponseType(typeof(IEnumerable<BaseVacancy>), 200)]
        [ProducesResponseType(404)]    
        public IActionResult GetAllBasic([FromQuery]int maxCount = 5)
        {
            return Ok(new List<BaseVacancy>());
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
        public IActionResult Get(int id)
        {
            return Ok(new Vacancy() { Id = id });
        }

        /// <summary>
        /// Adds the specified vacancy to the liked vacancies of the current user.
        /// </summary>
        /// <returns>The liked vacancy</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified vacancy was not found</response>   
        /// <response code="403">If the user was unauthorized</response>  
        [HttpPost]
        [Route("{id}/like")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public IActionResult LikeVacature()
        {
            return Ok();
        }
    }
}