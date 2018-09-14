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
    [Route("api/vacatures")]
    public class VacaturesController : Controller
    {
        /// <summary>
        /// Gets a list of all vacatures
        /// </summary>
        /// <param name="maxCount">The max amount of vacatures to return, defaults to 5</param>
        /// <returns>A list of all vacatures</returns>
        /// <response code="200">Returns the list of vacatures</response>
        /// <response code="404">If no vacatures are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Vacature>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5)
        {
            return Ok(new List<Vacature>());
        }

        /// <summary>
        /// Gets a list of all basic vacatures
        /// </summary>
        /// <returns>A list of all basic vacatures</returns>
        /// <param name="maxCount">The max amount of vacatures to return, defaults to 5</param>
        /// <response code="200">Returns the list of basic vacatures</response>
        /// <response code="404">If no basic vacatures are found</response>   
        [HttpGet]
        [Route("basic")]
        [ProducesResponseType(typeof(IEnumerable<BaseVacature>), 200)]
        [ProducesResponseType(404)]    
        public IActionResult GetAllBasic([FromQuery]int maxCount = 5)
        {
            return Ok(new List<BaseVacature>());
        }

        /// <summary>
        /// Gets a specific vacature by Id
        /// </summary>
        /// <param name="id">The Id of the vacature to retrieve</param>
        /// <returns>One specific vacature</returns>
        /// <response code="200">Returns the requested vacature</response>
        /// <response code="404">If the specified vacature was not found</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Vacature), 200)]
        [ProducesResponseType(404)]
        public IActionResult Get(int id)
        {
            return Ok(new Vacature() { Id = id });
        }

        /// <summary>
        /// Adds the specified vacature to the liked vacatures of the current user.
        /// </summary>
        /// <returns>The liked vacature</returns>
        /// <response code="200">If the request was a success</response>
        /// <response code="404">If the specified vacature was not found</response>   
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