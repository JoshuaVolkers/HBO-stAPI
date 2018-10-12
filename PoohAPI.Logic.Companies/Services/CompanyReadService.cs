using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using PoohAPI.Infrastructure.CompanyDB.Respositories;
using AutoMapper;
using PoohAPI.Infrastructure.CompanyDB.Models;
using Newtonsoft.Json;

namespace PoohAPI.Logic.Companies.Services
{
    public class CompanyReadService : ICompanyReadService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;
        private readonly IMapAPIReadService mapAPIReadService;

        public CompanyReadService(ICompanyRepository companyRepository, IMapper mapper, IMapAPIReadService mapAPIReadService)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.mapAPIReadService = mapAPIReadService;
        }

        public Company GetCompanyById(int id)
        {
            //var query = string.Format("SELECT b.*, l.land_naam " +
            //    "FROM reg_bedrijven b " +
            //    "INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id " +
            //    "WHERE b.bedrijf_id = {0}", id);

            //var query = "SELECT b.*, l.land_naam " +
            //    "FROM reg_bedrijven b " +
            //    "INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id " +
            //    "WHERE b.bedrijf_id = @id";

            var query = @"SELECT b.*, l.land_naam, 
                    IF(r.review_sterren IS NULL, 0,
                            CASE WHEN COUNT(r.review_sterren) > 4
                            THEN AVG(r.review_sterren)
                            ELSE 0 END
                       ) as average_reviews
                FROM reg_bedrijven b
                INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id
                LEFT JOIN reg_reviews r ON b.bedrijf_id = r.review_bedrijf_id
                WHERE b.bedrijf_id = @id AND b.bedrijf_actief = 1
                GROUP BY b.bedrijf_id";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            var dbCompany = this.companyRepository.GetCompany(query, parameters);
            
            return this.mapper.Map<Company>(dbCompany);
        }

        public IEnumerable<BaseCompany> GetListCompanies(int maxCount, int offset, double? minStars = null,
            double? maxStars = null, string cityName = null, string countryName = null, int? locationRange = null,
            string additionalLocationSearchTerms = null, int ? major = null, bool detailedCompanies = false)
        {
            
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);

            string starFilter = AddStarFilter(parameters, minStars, maxStars);
            string locationFilter = this.AddLocationFilter(parameters, countryName, additionalLocationSearchTerms, cityName, locationRange);
            string majorFilter = "";
            string majorJoin = "";

            if (!(major is null))
            {
                parameters.Add("@majorId", major);
                majorFilter = "AND ob.opb_opleiding_id = @majorId ";
                majorJoin = "INNER JOIN reg_opleiding_per_bedrijf ob ON b.bedrijf_id = ob.opb_bedrijf_id ";
            }

            string extraFields = "";

            if (detailedCompanies)
            {
                extraFields = "b.bedrijf_contactpersoon_email, b.bedrijf_website, b.bedrijf_social_linkedin, b.bedrijf_beschrijving,";
            }

            string query = @"
                SELECT 
                    b.bedrijf_id, b.bedrijf_handelsnaam, b.bedrijf_vestiging_straat, bedrijf_vestiging_huisnr, 
                    b.bedrijf_vestiging_toev, b.bedrijf_vestiging_postcode, b.bedrijf_vestiging_plaats, 
                    b.bedrijf_vestiging_land, l.land_naam, b.bedrijf_logo, b.bedrijf_breedtegraad, 
                    b.bedrijf_lengtegraad, " + extraFields + @" 
                    IF(r.review_sterren IS NULL, 0,
                            CASE WHEN COUNT(r.review_sterren) > 4
                            THEN AVG(r.review_sterren)
                            ELSE 0 END
                       ) as average_reviews
                FROM reg_bedrijven b
                INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id
                " + majorJoin + @"
                LEFT JOIN reg_reviews r ON b.bedrijf_id = r.review_bedrijf_id
                WHERE b.bedrijf_actief = 1 
                " + locationFilter + majorFilter + @" 
                GROUP BY b.bedrijf_id 
                " + starFilter + @" 
                LIMIT @limit OFFSET @offset";
            
            IEnumerable<DBCompany> dbCompanies = this.companyRepository.GetListCompanies(query, parameters);

            if (detailedCompanies)
            {
                return this.mapper.Map<IEnumerable<Company>>(dbCompanies);
            }
            else
            {
                return this.mapper.Map<IEnumerable<BaseCompany>>(dbCompanies);
            }
        }
        
        private string AddStarFilter(Dictionary<string, object> parameters, double? minStars = null, double? maxStars = null)
        {
            string starFilter = "";
            
            if (!(minStars is null) || !(maxStars is null))
            {
                starFilter += "HAVING ";

                if (!(minStars is null))
                {
                    starFilter += "average_reviews > @minStars ";
                    parameters.Add("@minStars", minStars);
                }

                if (!(minStars is null) && !(maxStars is null))
                {
                    starFilter += "AND ";
                }

                if (!(maxStars is null))
                {
                    starFilter += "average_reviews < @maxStars ";
                    parameters.Add("@maxStars", maxStars);
                }
            }

            return starFilter;
        }

        private string AddMajorFilter(Dictionary<string, object> parameters, int major)
        {
            parameters.Add("@majorId", major);
            return "AND ob.opb_opleiding_id = @majorId ";
        }

        private string AddLocationFilter(Dictionary<string, object> parameters, string countryName = null, 
            string municipalityName = null, string cityName = null, int? locationRange = null)
        {
            string locationFilter = "";

            if (!(cityName is null) && !(locationRange is null))
            {
                // Use Map API
                Coordinates coordinates = this.mapAPIReadService.GetMapCoordinates(cityName, countryName, municipalityName);

                if (!(coordinates is null))
                {
                    parameters.Add("@latitude", coordinates.Latitude);
                    parameters.Add("@longitude", coordinates.Longitude);
                    parameters.Add("@rangeKm", locationRange);

                    return @"AND (
                        6371 * acos(
                          cos(radians(@latitude))
                          * cos(radians(b.bedrijf_breedtegraad))
                          * cos(radians(b.bedrijf_lengtegraad) - radians(@longitude))
                          + sin(radians(@latitude))
                          * sin(radians(b.bedrijf_breedtegraad))
                        )
				      ) < @rangeKm";
                }
            }

            // Find matches in database
            if (!(cityName is null))
            {
                locationFilter += AddCityFilter(parameters, cityName);
            }

            if (!(countryName is null))
            {
                locationFilter += AddCountryFilter(parameters, countryName);
            }

            return locationFilter;
        }

        private string AddCountryFilter(Dictionary<string, object> parameters, string countryName)
        {
            parameters.Add("@countryName", countryName);
            return "AND l.land_naam = @countryName ";
        }

        private string AddCityFilter(Dictionary<string, object> parameters, string cityName)
        {
            parameters.Add("@cityName", cityName);
            return "AND b.bedrijf_vestiging_plaats = @cityName ";
        }

    }
}
