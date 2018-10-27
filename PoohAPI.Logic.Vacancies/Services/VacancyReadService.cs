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

namespace PoohAPI.Logic.Vacancies.Services
{
    class VacancyReadService : IVacancyReadService
    {
        private readonly IVacancyRepository vacancyRepository;
        private readonly IMapper mapper;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly IQueryBuilder queryBuilder;

        public VacancyReadService(IVacancyRepository vacancyRepository, IMapper mapper, IMapAPIReadService mapAPIReadService)
        {
            this.vacancyRepository = vacancyRepository;
            this.mapper = mapper;
            this.mapAPIReadService = mapAPIReadService;
            this.queryBuilder = new QueryBuilder();
        }

        public IEnumerable<Vacancy> GetListVacancies(int maxCount = 5, int offset = 0, string additionalLocationSearchTerms = null, int? educationid = null, int? educationalAttainmentid = null, IntershipType? intershipType = null, int? languageid = null, string cityName = null, string countryName = null, int? locationRange = null)
        {
            this.queryBuilder.Clear();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            this.AddVacancyBaseQuery();
            this.AddLimitAndOffset(parameters, maxCount, offset);
            this.AddLocationFilter(parameters, countryName, additionalLocationSearchTerms, cityName, locationRange);
            this.AddEducationFilter(parameters, educationid, educationalAttainmentid, intershipType);
            this.AddLanguageFilter(parameters, languageid);
            string query = this.queryBuilder.BuildQuery();
            this.queryBuilder.Clear();

            IEnumerable<DBVacancy> dbVacancies = this.vacancyRepository.GetListVacancies(query, parameters);

            return this.mapper.Map<IEnumerable<Vacancy>>(dbVacancies);
        }

        public Vacancy GetVacancyById(int id)
        {
            this.queryBuilder.Clear();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            this.AddVacancyBaseQuery();
            this.AddVacancyFilter(parameters, id);
            string query = this.queryBuilder.BuildQuery();

            this.queryBuilder.Clear();

            DBVacancy dBVacancy = this.vacancyRepository.GetVacancy(query, parameters);

            return this.mapper.Map<Vacancy>(dBVacancy);
        }

        public IEnumerable<Vacancy> GetFavoriteVacancies(int userid)
        {
            this.queryBuilder.Clear();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            this.AddVacancyBaseQuery();
            this.queryBuilder.AddJoinLine("INNER JOIN reg_vacatures_favoriet f ON v.vacature_id = f.vf_vacature_id");
            this.queryBuilder.AddWhere("f.vf_user_id = " + userid);

            string query = this.queryBuilder.BuildQuery();
            this.queryBuilder.Clear();

            IEnumerable<DBVacancy> dbVacancies = this.vacancyRepository.GetListVacancies(query, parameters);

            return this.mapper.Map<IEnumerable<Vacancy>>(dbVacancies);
        }

        private void AddLimitAndOffset(Dictionary<string, object> parameters, int maxCount, int offset)
        {
            this.queryBuilder.SetLimit("@limit");
            this.queryBuilder.SetOffset("@offset");

            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);
        }

        private void AddVacancyBaseQuery()
        {
            this.queryBuilder.AddSelect(@"v.vacature_id, v.vacature_bedrijf_id, v.vacature_user_id, v.vacature_titel, 
                                        v.vacature_plaats, v.vacature_datum_plaatsing, v.vacature_datum_verlopen, v.vacature_tekst,
                                        v.vacature_link, v.vacature_actief, v.vacature_breedtegraad, v.vacature_lengtegraad,
                                        t.talen_naam, n.opn_naam, GROUP_CONCAT(DISTINCT o.opl_id,'-',o.opl_naam) as opleidingen, b.bedrijf_vestiging_land, b.bedrijf_vestiging_plaats, b.bedrijf_vestiging_straat, b.bedrijf_vestiging_huisnr, b.bedrijf_vestiging_toev, b.bedrijf_vestiging_postcode, l.land_naam, s.stagesoort");

            this.queryBuilder.SetFrom("reg_vacatures v");

            this.queryBuilder.AddJoinLine("INNER JOIN reg_talen t ON v.vacature_taal = t.talen_id");

            this.queryBuilder.AddJoinLine("INNER JOIN reg_opleidingsniveau n ON v.vacature_op_niveau = n.opn_id");

            this.queryBuilder.AddJoinLine("INNER JOIN reg_vacatures_opleidingen r ON v.vacature_id = r.rvo_vacature_id");
            this.queryBuilder.AddJoinLine("INNER JOIN reg_opleidingen o ON r.rvo_opleiding_id = o.opl_id");

            this.queryBuilder.AddJoinLine("INNER JOIN reg_stagesoort s ON v.vacature_stagesoort = s.stage_id");

            this.queryBuilder.AddJoinLine("INNER JOIN reg_bedrijven b ON v.vacature_bedrijf_id = b.bedrijf_id");
            this.queryBuilder.AddJoinLine("INNER JOIN reg_landen l ON b.bedrijf_vestiging_land = l.land_id");

            this.queryBuilder.AddWhere("v.vacature_actief = 1");
            this.queryBuilder.AddGroupBy("v.vacature_id");
        }



        private void AddLocationFilter(Dictionary<string, object> parameters, string countryName = null,
    string municipalityName = null, string cityName = null, int? locationRange = null)
        {
            if (!(cityName is null) && !(locationRange is null))
            {
                // Use Map API
                Coordinates coordinates = this.mapAPIReadService.GetMapCoordinates(cityName, countryName, municipalityName);

                if (!(coordinates is null))
                {
                    parameters.Add("@latitude", coordinates.Latitude);
                    parameters.Add("@longitude", coordinates.Longitude);
                    parameters.Add("@rangeKm", locationRange);

                    this.queryBuilder.AddSelect(@"(
                        6371 * acos(
                          cos(radians(@latitude))
                          * cos(radians(v.vacature_breedtegraad))
                          * cos(radians(v.vacature_lengtegraad) - radians(@longitude))
                          + sin(radians(@latitude))
                          * sin(radians(v.vacature_breedtegraad))
                        )) as distance");
                    this.queryBuilder.AddHaving("distance < @rangeKm");
                }
            }
            else
            {
                // Find matches in database
                if (!(cityName is null))
                {
                    AddCityFilter(parameters, cityName);
                }

                if (!(countryName is null))
                {
                    AddCountryFilter(parameters, countryName);
                }
            }
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
                this.queryBuilder.AddHaving("opleidingen LIKE @educationid");
                parameters.Add("@educationid", String.Format("%{0}%", educationid));
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
    }
}
