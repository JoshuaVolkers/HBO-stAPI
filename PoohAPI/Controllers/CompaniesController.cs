using Microsoft.AspNetCore.Mvc;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using System.Collections.Generic;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("companies")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyReadService companyReadService;
        private readonly ICompanyCommandService companyCommandService;

        public CompaniesController(ICompanyReadService companyReadService, ICompanyCommandService companyCommandService)
        {
            this.companyReadService = companyReadService;
            this.companyCommandService = companyCommandService;
        }

        /// <summary>
        /// Gets a list of Companies
        /// </summary>
        /// <remarks>Returns Companies or BaseCompanies. The model used is determined by detailedCompanies</remarks>
        /// <param name="maxCount">The max amount of companies to return, defaults to 5</param>
        /// <param name="offset">The number of companies to skip</param>
        /// <param name="minStars">The min number of stars that the returning companies should have</param>
        /// <param name="maxStars">The max number of stars that the returning companies should have</param>
        /// <param name="cityName">The city in which the companies should be located</param>
        /// <param name="countryId">The id of the country where the company is located. Country ids can be obtained from the options/countries endpoint.</param>
        /// <param name="major">The major which the returning companies should be suitable for</param>
        /// <param name="languages">A comma seperated list of the languages to get companies for</param>
        /// <param name="detailedCompanies">The type of model to return, false = BaseCompany, true = Company. Set to true to retrieve more details.</param>
        /// <returns>A list of all basicCompanies</returns>
        /// <response code="200">Returns the list of basicCompanies</response>
        /// <response code="404">If no companies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Company>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5, [FromQuery]int offset = 0, [FromQuery]double? minStars = null,
            [FromQuery]double? maxStars = null, [FromQuery]string cityName = null, [FromQuery]int? countryId = null,
            [FromQuery]string major = null, [FromQuery]string[] languages = null, [FromQuery]bool detailedCompanies = false )
        {
            if (minStars < 0 || minStars > 5 || maxStars < 0 || maxStars > 5)
            {
                return BadRequest("Number of stars should be between 0 and 5");
            }

            if (detailedCompanies)
                return Ok(new List<Company>());
            return Ok(new List<BaseCompany>());
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
            Company company = this.companyReadService.GetCompanyById(id);

            if (company is Company)
            {
                return Ok(this.companyReadService.GetCompanyById(id));
            }
            else
            {
                return NotFound("Company not found.");
            }
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
            //TODO: we should be able to return both anonymous and normal reviews based on the choice the student made when posting it.
            return Ok(new List<ReviewAnonymous>());
        }
    }
}
