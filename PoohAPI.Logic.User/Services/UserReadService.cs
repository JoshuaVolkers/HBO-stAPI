using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using System;
using System.Collections.Generic;
using PoohAPI.Logic.Common.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Users.Services
{
    public class UserReadService : IUserReadService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IQueryBuilder queryBuilder;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly IReviewReadService reviewReadService;

        public UserReadService(IUserRepository userRepository, IMapper mapper, IQueryBuilder queryBuilder, IMapAPIReadService mapAPIReadService, IReviewReadService reviewReadService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            this.queryBuilder = queryBuilder;
            this.mapAPIReadService = mapAPIReadService;
            this.reviewReadService = reviewReadService;
        }

        public IEnumerable<BaseUser> GetAllUsers(int maxCount, int offset, string educationalAttainment = null,
            string educations = null, string cityName = null, string countryName = null, int? range = null,
            string additionalLocationSearchTerms = null, int? preferredLanguage = null)
        {
            //var query = "SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
            //                          " FROM reg_users LIMIT @maxCount OFFSET @offset";

            //var parameters = new Dictionary<string, object>
            //{
            //    { "@maxCount", maxCount },
            //    { "@offset", offset }
            //};

            //var users = _userRepository.GetAllUsers(query, parameters);
            //return _mapper.Map<IEnumerable<User>>(users);

            this.queryBuilder.Clear();

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            this.queryBuilder.AddSelect("u.user_id, u.user_email, u.user_name, u.user_role");
            this.queryBuilder.SetFrom("reg_users u");
            this.queryBuilder.AddJoinLine("INNER JOIN reg_user_studenten s ON u.user_id = s.user_id");
            this.queryBuilder.SetLimit("@limit");
            this.queryBuilder.SetOffset("@offset");

            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);

            this.AddLocationFilter(parameters, countryName, additionalLocationSearchTerms, cityName, range);
            this.AddEducationsFilter(parameters, educations);
            this.AddEducationalAttainmentIds(parameters, educationalAttainment);
            this.AddPreferredLanguageFilter(parameters, preferredLanguage);

            string query = this.queryBuilder.BuildQuery();
            this.queryBuilder.Clear();

            var users = _userRepository.GetAllUsers(query, parameters);
            return _mapper.Map<IEnumerable<BaseUser>>(users);
        }

        public User GetUserByEmail(string email)
        {
            var query = "SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
                        " FROM reg_users WHERE user_email = @email";
            var parameters = new Dictionary<string, object>
            {
                {"@email", email}
            };

            var user = _userRepository.GetUser(query, parameters);

            return _mapper.Map<User>(user);
        }

        public User GetUserById(int id)
        {
            //var query = string.Format("SELECT * FROM wp_dev_users WHERE ID = {0}", id);

            string query = @"
                SELECT u.user_id, u.user_email, u.user_name, s.user_land, s.user_woonplaats, s.user_opleiding_id, 
                    s.user_op_niveau, s.user_taal, s.user_breedtegraad, s.user_lengtegraad, 
                    IF(l.land_naam IS NULL, '', l.land_naam) AS land_naam, IF(t.talen_naam IS NULL, '', t.talen_naam) AS talen_naam, 
                    IF(o.opl_naam IS NULL, '', o.opl_naam) AS opl_naam, IF(op.opn_naam IS NULL, '', op.opn_naam) AS opn_naam 
                FROM reg_users u 
                INNER JOIN reg_user_studenten s ON u.user_id = s.user_id
                LEFT JOIN reg_landen l ON s.user_land = l.land_id
                LEFT JOIN reg_talen t ON s.user_taal = t.talen_id
                LEFT JOIN reg_opleidingen o ON s.user_opleiding_id = o.opl_id
                LEFT JOIN reg_opleidingsniveau op ON s.user_op_niveau = op.opn_id
                WHERE u.user_role = 0 AND u.user_active = 1 AND u.user_id = @id
                ";

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            var dbUser = _userRepository.GetUser(query, parameters);
            User user = _mapper.Map<User>(dbUser);

            if (user is null)
                return user;

            user.Reviews = this.reviewReadService.GetListReviewIdsForUser(user.Id);

            return user;
        }

        public User Login(string email, string password)
        {
            var query = "SELECT * FROM reg_users WHERE user_email = @email AND user_account_type = @type";
            var parameters = new Dictionary<string, object>
            {
                { "@email", email },
                { "@type", (int)UserAccountType.ApiUser}
            };

            var user = _userRepository.GetUser(query, parameters);

            if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.user_password))
                return _mapper.Map<User>(user);

            return null;
        }

        private void AddEducationsFilter(Dictionary<string, object> parameters, string educationsIds)
        {
            if (educationsIds is null)
                return;

            List<string> splitIds = educationsIds.Split(',').ToList();
            List<int> ids = this.CreateIdList(splitIds);

            if (ids.Count <= 0)
                return;

            string educationOr = "(";

            for (int i = 0; i < ids.Count; i++)
            {
                parameters.Add("@eid" + i.ToString(), ids[i]);
                educationOr += "s.user_opleiding_id = @eid" + i.ToString() + " ";

                if (i != (ids.Count - 1))
                    educationOr += "OR ";
            }

            educationOr += ")";

            this.queryBuilder.AddWhere(educationOr);
        }

        private void AddEducationalAttainmentIds(Dictionary<string, object> parameters, string educationalAttainmentIds)
        {
            if (educationalAttainmentIds is null)
                return;

            List<string> splitIds = educationalAttainmentIds.Split(',').ToList();
            List<int> ids = this.CreateIdList(splitIds);

            if (ids.Count <= 0)
                return;

            string educationOr = "(";

            for (int i = 0; i < ids.Count; i++)
            {
                parameters.Add("@aid" + i.ToString(), ids[i]);
                educationOr += "s.user_op_niveau = @aid" + i.ToString() + " ";

                if (i != (ids.Count - 1))
                    educationOr += "OR ";
            }

            educationOr += ")";

            this.queryBuilder.AddWhere(educationOr);
        }

        /// <summary>
        /// Create list of int ids from list of string ids.
        /// Ids are parsed to integers and checked on having a value larger than 0.
        /// </summary>
        /// <param name="splitIds"></param>
        /// <returns></returns>
        private List<int> CreateIdList(List<string> splitIds)
        {
            List<int> ids = new List<int>();

            foreach (string splitId in splitIds)
            {
                int id;
                bool success = Int32.TryParse(splitId, out id);
                if (success)
                {
                    if (id > 0)
                        ids.Add(id);
                }
            }

            return ids;
        }

        private void AddPreferredLanguageFilter(Dictionary<string, object> parameters, int? preferredLanguage)
        {
            if (preferredLanguage is null || preferredLanguage <= 0)
                return;

            this.queryBuilder.AddWhere("s.user_taal = @language");
            parameters.Add("@language", preferredLanguage);
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
                          * cos(radians(s.user_breedtegraad))
                          * cos(radians(s.user_lengtegraad) - radians(@longitude))
                          + sin(radians(@latitude))
                          * sin(radians(s.user_breedtegraad))
                        )) as distance");
                    this.queryBuilder.AddHaving("distance < @rangeKm");
                }
            }
            else
            {
                // Find matches in database
                if (!(cityName is null))
                    this.AddCityFilter(parameters, cityName);

                if (!(countryName is null))
                    this.AddCountryFilter(parameters, countryName);
            }
        }

        private void AddCountryFilter(Dictionary<string, object> parameters, string countryName)
        {
            this.queryBuilder.AddWhere("l.land_naam = @countryName");
            this.queryBuilder.AddJoinLine("INNER JOIN reg_landen l ON s.user_land = l.land_id");
            parameters.Add("@countryName", countryName);
        }

        private void AddCityFilter(Dictionary<string, object> parameters, string cityName)
        {
            this.queryBuilder.AddWhere("s.user_woonplaats = @cityName");
            parameters.Add("@cityName", cityName);
        }
    }
}
