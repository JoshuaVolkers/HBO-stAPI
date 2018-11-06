using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using PoohAPI.Logic.Common.Classes;
using PoohAPI.Infrastructure.CompanyDB.Respositories;
using AutoMapper;
using PoohAPI.Infrastructure.CompanyDB.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace PoohAPI.Logic.Companies.Services
{
    public class CompanyReadService : ICompanyReadService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly IQueryBuilder queryBuilder;
        private readonly IConfiguration config;

        public CompanyReadService(ICompanyRepository companyRepository, IMapper mapper, IMapAPIReadService mapAPIReadService, IConfiguration config)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.mapAPIReadService = mapAPIReadService;
            this.queryBuilder = new QueryBuilder();
            this.config = config;
        }

        /// <summary>
        /// Get a single company by id. Only active companies will be returned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Company GetCompanyById(int id)
        {
            string query = @"SELECT b.*, l.land_naam, GROUP_CONCAT(DISTINCT o.opl_naam) as opleidingen, 
                    IF(r.review_sterren IS NULL, 0,
                            CASE WHEN COUNT(r.review_sterren) > 4
                            THEN AVG(r.review_sterren)
                            ELSE 0 END
                       ) as average_reviews
                FROM reg_bedrijven b
                LEFT JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id
                LEFT JOIN reg_reviews r ON b.bedrijf_id = r.review_bedrijf_id
                LEFT JOIN reg_opleiding_per_bedrijf ob ON b.bedrijf_id = ob.opb_bedrijf_id
                LEFT JOIN reg_opleidingen o ON ob.opb_opleiding_id = o.opl_id
                WHERE b.bedrijf_id = @id AND b.bedrijf_actief = 1
                GROUP BY b.bedrijf_id";
            
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            DBCompany dbCompany = this.companyRepository.GetCompany(query, parameters);
            
            return this.mapper.Map<Company>(dbCompany);
        }

        /// <summary>
        /// Get a list of companies. Filters can be included. Only active companies will be returned.
        /// </summary>
        /// <param name="maxCount">Maximum number of companies to retrieve</param>
        /// <param name="offset"></param>
        /// <param name="minStars">Minimum number of stars the companies should have</param>
        /// <param name="maxStars">Maximum number of stars the companies should have</param>
        /// <param name="cityName">The name of the city in which the companies should be located</param>
        /// <param name="countryName">The name of the country the companies should be located</param>
        /// <param name="locationRange">The range around the city in which the companies should be located</param>
        /// <param name="additionalLocationSearchTerms">Search terms if there are more cities with the same name within the country</param>
        /// <param name="major">The major which the companies should be approved for</param>
        /// <param name="detailedCompanies">Whether or not the companies should have detailed information</param>
        /// <returns></returns>
        public IEnumerable<BaseCompany> GetListCompanies(int maxCount, int offset, double? minStars = null,
            double? maxStars = null, string cityName = null, string countryName = null, int? locationRange = null,
            string additionalLocationSearchTerms = null, int? major = null, bool detailedCompanies = false)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            
            this.AddCompanyBaseQuery(parameters, maxCount, offset);
            this.AddStarFilter(parameters, minStars, maxStars);
            this.AddLocationFilter(parameters, countryName, additionalLocationSearchTerms, cityName, locationRange);
            this.AddMajorFilter(parameters, major);

            if (detailedCompanies)
                this.queryBuilder.AddSelect("b.bedrijf_contactpersoon_email, b.bedrijf_website, b.bedrijf_social_linkedin, b.bedrijf_beschrijving");

            string query = this.queryBuilder.BuildQuery();
            
            IEnumerable<DBCompany> dbCompanies = this.companyRepository.GetListCompanies(query, parameters);

            if (detailedCompanies)
                return this.mapper.Map<IEnumerable<Company>>(dbCompanies);
            else
                return this.mapper.Map<IEnumerable<BaseCompany>>(dbCompanies);
        }

        private void AddMajorFilter(Dictionary<string, object> parameters, int? major)
        {
            if (major != null)
            {
                this.queryBuilder.AddJoinLine("INNER JOIN reg_opleiding_per_bedrijf obf ON b.bedrijf_id = obf.opb_bedrijf_id");
                this.queryBuilder.AddWhere("obf.opb_opleiding_id = @majorId");
                parameters.Add("@majorId", major);
            }
        }

        private void AddCompanyBaseQuery(Dictionary<string, object> parameters, int maxCount, int offset)
        {
            this.queryBuilder.AddSelect(@"b.bedrijf_id, b.bedrijf_handelsnaam, b.bedrijf_vestiging_straat, bedrijf_vestiging_huisnr, 
                    b.bedrijf_vestiging_toev, b.bedrijf_vestiging_postcode, b.bedrijf_vestiging_plaats,
                    b.bedrijf_vestiging_land, l.land_naam, b.bedrijf_logo, b.bedrijf_breedtegraad,
                    b.bedrijf_lengtegraad, GROUP_CONCAT(DISTINCT o.opl_naam) as opleidingen");
            this.queryBuilder.AddSelect(@" 
                    IF(r.review_sterren IS NULL, 0,
                            CASE WHEN COUNT(r.review_sterren) > 4
                            THEN AVG(r.review_sterren)
                            ELSE 0 END
                       ) as average_reviews");
            this.queryBuilder.SetFrom("reg_bedrijven b");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_reviews r ON b.bedrijf_id = r.review_bedrijf_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_opleiding_per_bedrijf ob ON b.bedrijf_id = ob.opb_bedrijf_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_opleidingen o ON ob.opb_opleiding_id = o.opl_id");
            this.queryBuilder.AddWhere("b.bedrijf_actief = 1");
            this.queryBuilder.AddGroupBy("b.bedrijf_id");
            this.queryBuilder.SetLimit("@limit");
            this.queryBuilder.SetOffset("@offset");

            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);
        }

        private void AddStarFilter(Dictionary<string, object> parameters, double? minStars = null, double? maxStars = null)
        {
            if (!(minStars is null))
            {
                this.queryBuilder.AddHaving("average_reviews > @minStars");
                parameters.Add("@minStars", minStars);
            }

            if (!(maxStars is null))
            {
                this.queryBuilder.AddHaving("average_reviews < @maxStars");
                parameters.Add("@maxStars", maxStars);
            }
        }

        private void AddLocationFilter(Dictionary<string, object> parameters, string countryName = null, 
            string municipalityName = null, string cityName = null, int? locationRange = null)
        {
            if (cityName != null && locationRange != null)
            {
                // Use Map API
                Coordinates coordinates = this.mapAPIReadService.GetMapCoordinates(cityName, countryName, municipalityName);

                if (coordinates != null)
                {
                    parameters.Add("@latitude", coordinates.Latitude);
                    parameters.Add("@longitude", coordinates.Longitude);
                    parameters.Add("@rangeKm", locationRange);

                    // Select companies within the range. The formula is called a haversine formula.
                    this.queryBuilder.AddSelect(@"(
                        6371 * acos(
                          cos(radians(@latitude))
                          * cos(radians(b.bedrijf_breedtegraad))
                          * cos(radians(b.bedrijf_lengtegraad) - radians(@longitude))
                          + sin(radians(@latitude))
                          * sin(radians(b.bedrijf_breedtegraad))
                        )) as distance");
                    this.queryBuilder.AddHaving("distance < @rangeKm");
                }
                else
                {
                    // Find matches in database
                    this.AddLocationFilterWithoutCoordinates(parameters, countryName, cityName);
                }
            }
            else
            {
                // Find matches in database
                this.AddLocationFilterWithoutCoordinates(parameters, countryName, cityName);
            }
        }

        private void AddLocationFilterWithoutCoordinates(Dictionary<string, object> parameters, string countryName, string cityName)
        {
            if (cityName != null)
                AddCityFilter(parameters, cityName);

            if (countryName != null)
                AddCountryFilter(parameters, countryName);
        }

        private void AddCountryFilter(Dictionary<string, object> parameters, string countryName)
        {
            this.queryBuilder.AddWhere("l.land_naam = @countryName");
            parameters.Add("@countryName", countryName);
        }

        private void AddCityFilter(Dictionary<string, object> parameters, string cityName)
        {
            this.queryBuilder.AddWhere("b.bedrijf_vestiging_plaats = @cityName");
            parameters.Add("@cityName", cityName);
        }

    }
}
