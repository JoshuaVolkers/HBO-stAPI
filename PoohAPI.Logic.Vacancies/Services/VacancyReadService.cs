using AutoMapper;
using PoohAPI.Infrastructure.VacancyDB.Models;
using PoohAPI.Infrastructure.VacancyDB.Repositories;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.MapAPI.Helpers;

namespace PoohAPI.Logic.Vacancies.Services
{
    class VacancyReadService : IVacancyReadService
    {
        private readonly IVacancyRepository vacancyRepository;
        private readonly IMapper mapper;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly LocationHelper locationHelper;
        private IQueryBuilder queryBuilder;

        public VacancyReadService(IVacancyRepository vacancyRepository, IMapper mapper, IMapAPIReadService mapAPIReadService)
        {
            this.vacancyRepository = vacancyRepository;
            this.mapper = mapper;
            this.mapAPIReadService = mapAPIReadService;
            this.queryBuilder = new QueryBuilder();
            this.locationHelper = new LocationHelper();
        }

        public IEnumerable<Vacancy> GetListVacancies(int maxcount = 5, int offset = 0, string additionallocationsearchterms = null, int? educationid = null, int? educationalattainmentid = null, IntershipType? internshiptype = null, int? languageid = null, string cityname = null, string countryname = null, int? locationrange = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //The base query without any filters
            this.AddVacancyBaseQuery();

            //Adding the maxlimit and the offset to the query
            this.AddLimitAndOffset(parameters, maxcount, offset);

            //Adding the location filtering to the query
            this.AddLocationFilter(parameters, countryname, additionallocationsearchterms, cityname, locationrange);

            //Adding the education, educationalattainment and internshiptype to the filter
            this.AddEducationFilter(parameters, educationid, educationalattainmentid, internshiptype);

            //Adding language filter to the query
            this.AddLanguageFilter(parameters, languageid);

            //building the query
            string query = this.queryBuilder.BuildQuery();

            //Retrieving the vacancies from the database
            IEnumerable<DBVacancy> dbVacancies = this.vacancyRepository.GetListVacancies(query, parameters);

            //Returning the list of DB vacancies as a list of Vacancies using automapper
            return this.mapper.Map<IEnumerable<Vacancy>>(dbVacancies);
        }

        public Vacancy GetVacancyById(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //Adding the base vacancy query
            this.AddVacancyBaseQuery();

            //Filtering on the specified vacancy with id
            this.AddVacancyFilter(parameters, id);

            //Build query
            string query = this.queryBuilder.BuildQuery();

            //Retrieve single vacancy
            DBVacancy dBVacancy = this.vacancyRepository.GetVacancy(query, parameters);

            //Return single vancancy as vacancy object using automapper
            return this.mapper.Map<Vacancy>(dBVacancy);
        }

        public IEnumerable<Vacancy> GetFavoriteVacancies(int userid)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //Base vacancy for getting vacancies
            this.AddVacancyBaseQuery();

            //Join for joining vacancies on the user's favorite vacancies
            this.queryBuilder.AddJoinLine("INNER JOIN reg_vacatures_favoriet f ON v.vacature_id = f.vf_vacature_id");

            //Whereclause to filter on the userid
            this.queryBuilder.AddWhere("f.vf_user_id = " + userid);

            //Build the query
            string query = this.queryBuilder.BuildQuery();

            //Retrieve the vacancies
            IEnumerable<DBVacancy> dbVacancies = this.vacancyRepository.GetListVacancies(query, parameters);

            //Return the list of vacancies as a vacancy list using automapper
            return this.mapper.Map<IEnumerable<Vacancy>>(dbVacancies);
        }
        public IEnumerable<int> GetListVacancyIdsForUser(int userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);

            string query = @"SELECT vacature_id 
                             FROM reg_vacatures v
                             INNER JOIN reg_vacatures_favoriet f ON v.vacature_id = f.vf_vacature_id
                             WHERE vf_user_id = @id";

            return this.mapper.Map<IEnumerable<int>>(this.vacancyRepository.GetListVacancyIds(query, parameters));
        }

        private void AddVacancyBaseQuery()
        {
            //Select query with the education as a group concat to get the results as 1 column seperated by a comma
            this.queryBuilder.AddSelect(@"v.vacature_id, v.vacature_bedrijf_id, v.vacature_user_id, v.vacature_titel, 
                                        v.vacature_plaats, v.vacature_datum_plaatsing, v.vacature_datum_verlopen, v.vacature_tekst,
                                        v.vacature_link, v.vacature_actief, v.vacature_breedtegraad, v.vacature_lengtegraad,
                                        t.talen_naam, n.opn_naam, GROUP_CONCAT(DISTINCT o.opl_id) as opleidingen, 
                                        b.bedrijf_vestiging_land, b.bedrijf_vestiging_plaats, b.bedrijf_vestiging_straat, b.bedrijf_vestiging_huisnr, 
                                        b.bedrijf_vestiging_toev, b.bedrijf_vestiging_postcode, l.land_naam, s.stagesoort");

            //From the vacancies table
            this.queryBuilder.SetFrom("reg_vacatures v");

            //Join for the languages
            this.queryBuilder.AddJoinLine("INNER JOIN reg_talen t ON v.vacature_taal = t.talen_id");

            //Join for the educationalattainment
            this.queryBuilder.AddJoinLine("INNER JOIN reg_opleidingsniveau n ON v.vacature_op_niveau = n.opn_id");

            //Join for the educations (2 joins for the group concat to work)
            this.queryBuilder.AddJoinLine("INNER JOIN reg_vacatures_opleidingen r ON v.vacature_id = r.rvo_vacature_id");
            this.queryBuilder.AddJoinLine("INNER JOIN reg_opleidingen o ON r.rvo_opleiding_id = o.opl_id");

            //Join for the companies
            this.queryBuilder.AddJoinLine("INNER JOIN reg_bedrijven b ON v.vacature_bedrijf_id = b.bedrijf_id");

            //Join for the countries
            this.queryBuilder.AddJoinLine("INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id");

            //Join for the internshiptype
            this.queryBuilder.AddJoinLine("INNER JOIN reg_stagesoort s ON v.vacature_stagesoort = s.stage_id");

            //Where clause to only get active vacancies
            this.queryBuilder.AddWhere("v.vacature_actief = 1");

            //Group by vacancyid needed for group concat to work
            this.queryBuilder.AddGroupBy("v.vacature_id");
        }

        private void AddLimitAndOffset(Dictionary<string, object> parameters, int maxCount, int offset)
        {
            this.queryBuilder.SetLimit("@limit");
            this.queryBuilder.SetOffset("@offset");

            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);
        }

        private void AddLocationFilter(Dictionary<string, object> parameters, string countryName = null,
                                       string municipalityName = null, string cityName = null, int? locationRange = null)
        {
            locationHelper.AddLocationFilter(ref parameters, mapAPIReadService, ref queryBuilder, 'v', "vacature", countryName, municipalityName, cityName, locationRange);
        }

        private void AddVacancyFilter(Dictionary<string, object> parameters, int id)
        {
            this.queryBuilder.AddWhere("v.vacature_id = @id");
            parameters.Add("@id", id);
        }

        private void AddEducationFilter(Dictionary<string, object> parameters, int? educationid = null, int? educationalAttainmentid = null, IntershipType? intershipType = null)
        {
            if(educationalAttainmentid != null)
            {
                this.queryBuilder.AddWhere("n.opn_id = @educationalAttainmentid");
                parameters.Add("@educationalAttainmentid", educationalAttainmentid);
            }

            if(educationid != null)
            {
                this.queryBuilder.AddHaving(String.Format("FIND_IN_SET( {0}, opleidingen ) > 0", educationid));
            }

            if(intershipType != null)
            {
                this.queryBuilder.AddWhere("s.stagesoort = @intershipType");
                parameters.Add("@intershipType", intershipType.Value.ToString());
            }
        }

        private void AddLanguageFilter(Dictionary<string, object> parameters, int? languageid = null)
        {
            if(languageid != null)
            {
                this.queryBuilder.AddWhere("t.talen_id = @languageid");
                parameters.Add("@languageid", languageid);
            }
        }
    }
}
