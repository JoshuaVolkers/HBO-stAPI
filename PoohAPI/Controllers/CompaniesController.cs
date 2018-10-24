using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using PoohAPI.Logic.Common.Models.PresentationModels;
using System.Collections.Generic;

namespace PoohAPI.Controllers
{
    [Produces("application/json")]
    [Route("companies")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyReadService companyReadService;
        private readonly ICompanyCommandService companyCommandService;
        private readonly IReviewReadService reviewReadService;
        private readonly IConfiguration config;

        public CompaniesController(ICompanyReadService companyReadService, ICompanyCommandService companyCommandService,
            IReviewReadService reviewReadService, IConfiguration config)
        {
            this.companyReadService = companyReadService;
            this.companyCommandService = companyCommandService;
            this.reviewReadService = reviewReadService;
            this.config = config;
        }

        /// <summary>
        /// Gets a list of Companies
        /// </summary>
        /// <remarks>Returns Companies or BaseCompanies. The model used is determined by detailedCompanies</remarks>
        /// <param name="maxCount">The max amount of companies to return, defaults to 5</param>
        /// <param name="offset">The number of companies to skip</param>
        /// <param name="minStars">The min number of stars that the returning companies should have</param>
        /// <param name="maxStars">The max number of stars that the returning companies should have</param>
        /// <param name="cityName">The city in which the companies should be located. CountryId is required for this filter to work.</param>
        /// <param name="countryName">The name of the country where the company is located. Country names can be obtained from the options/countries endpoint.</param>
        /// <param name="locationRange">The search range from the city. (CityName is required for this filter to work.)</param>
        /// <param name="additionalLocationSearchTerms">Additional search terms (municipality/province/state) if there are multiple cities with the same name in the same country. Separate terms by spaces. (LocationRange is required for this filter to work.)</param>
        /// <param name="major">The major which the returning companies should be suitable for</param>
        /// <param name="detailedCompanies">The type of model to return, false = BaseCompany, true = Company. Set to true to retrieve more details.</param>
        /// <returns>A list of all basicCompanies</returns>
        /// <response code="200">Returns the list of basicCompanies</response>
        /// <response code="404">If no companies are found</response>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Company>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAll([FromQuery]int maxCount = 5, [FromQuery]int offset = 0, [FromQuery]double? minStars = null,
            [FromQuery]double? maxStars = null, [FromQuery]string cityName = null, [FromQuery]string countryName = null,
            [FromQuery]int? locationRange = null, [FromQuery]string additionalLocationSearchTerms = null, [FromQuery]int? major = null, [FromQuery]bool detailedCompanies = false )
        {
            if (maxCount < 1 || maxCount > 100)
                return BadRequest("MaxCount should be between 1 and 100");
            if (offset < 0)
                return BadRequest("Offset should be 0 or larger");
            if (minStars < 1 || minStars > 5 || maxStars < 1 || maxStars > 5)
                return BadRequest("Number of stars should be between 1 and 5");

            IEnumerable<BaseCompany> companies = this.companyReadService.GetListCompanies(maxCount, offset, minStars, maxStars,
                cityName, countryName, locationRange, additionalLocationSearchTerms, major, detailedCompanies);

            if (companies is null)
                return NotFound("No companies were found");

            return Ok(companies);
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
            string testValue = this.config.GetValue<string>("TestValue");
            string testToken = this.config.GetSection("JWTSettings").GetValue<string>("JWTSigningKey");

            Company company = this.companyReadService.GetCompanyById(id);
            
            if (company is null)
                return NotFound("Company not found.");

            return Ok(company);
        }

        /// <summary>
        /// Retrieves anonymous reviews for a company by its id
        /// </summary>
        /// <param name="id">The id of a company</param>
        /// <returns>A list of anonymous reviews</returns>
        /// <response code="200">Returns the anonymous reviews of the company</response>
        /// <response code="404">If the specified company was not found</response>   
        [HttpGet("{id}/reviews")]
        [ProducesResponseType(typeof(IEnumerable<ReviewPublicPresentation>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetCompanyReviews(int id)
        {
            //TODO: we should be able to return both anonymous and normal reviews based on the choice the student made when posting it.
            IEnumerable<ReviewPublicPresentation> reviews = this.reviewReadService.GetListReviewsForCompany(id);

            if (reviews is null)
                return NotFound("No reviews found for this company.");

            return Ok(reviews);
        }
    }
}
