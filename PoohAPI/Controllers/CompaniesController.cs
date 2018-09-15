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
        /// <returns>A list of all companies</returns>
        /// <response code="200">Returns the list of companies</response>
        /// <response code="404">If no companies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Company>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5)
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
    }
}
