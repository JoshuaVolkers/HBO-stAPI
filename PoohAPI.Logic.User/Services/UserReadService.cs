using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System;
using System.Collections.Generic;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Classes;
using System.Linq;

namespace PoohAPI.Logic.Users.Services
{
    public class UserReadService : IUserReadService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IQueryBuilder queryBuilder;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly IReviewReadService reviewReadService;
        private readonly IVacancyReadService vacancyReadService;

        public UserReadService(IUserRepository userRepository, IMapper mapper,
            IMapAPIReadService mapAPIReadService, IReviewReadService reviewReadService, IVacancyReadService vacancyReadService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            this.queryBuilder = new QueryBuilder();
            this.mapAPIReadService = mapAPIReadService;
            this.reviewReadService = reviewReadService;
            this.vacancyReadService = vacancyReadService;
        }

        public IEnumerable<User> GetAllUsers(int maxCount, int offset, string educationLevels = null,
            string majors = null, string cityName = null, string countryName = null, int? range = null,
            string additionalLocationSearchTerms = null, int? preferredLanguage = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            this.queryBuilder.AddSelect(@"u.user_id, u.user_email, u.user_name, s.user_land, s.user_woonplaats, s.user_opleiding_id, 
                    s.user_op_niveau, s.user_taal, s.user_breedtegraad, s.user_lengtegraad,
                    l.land_naam, t.talen_naam, o.opl_naam, op.opn_naam, u.user_active");
            this.queryBuilder.SetFrom("reg_users u");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_user_studenten s ON u.user_id = s.user_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_landen l ON s.user_land = l.land_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_talen t ON s.user_taal = t.talen_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_opleidingen o ON s.user_opleiding_id = o.opl_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_opleidingsniveau op ON s.user_op_niveau = op.opn_id");
            this.queryBuilder.AddWhere("u.user_role = 0");
            this.queryBuilder.AddWhere("u.user_active = 1");
            this.queryBuilder.SetLimit("@limit");
            this.queryBuilder.SetOffset("@offset");

            parameters.Add("@limit", maxCount);
            parameters.Add("@offset", offset);

            this.AddLocationFilter(parameters, countryName, additionalLocationSearchTerms, cityName, range);
            this.AddMajorsFilter(parameters, majors);
            this.AddEducationLevelsFilter(parameters, educationLevels);
            this.AddPreferredLanguageFilter(parameters, preferredLanguage);

            string query = this.queryBuilder.BuildQuery();

            var users = _userRepository.GetAllUsers(query, parameters);
            return _mapper.Map<IEnumerable<User>>(users);
        }

        public T GetUserByEmail<T>(string email)
        {     
            var type = typeof(T);
            if (type == typeof(JwtUser))
                this.queryBuilder.AddSelect(@"user_id, user_name, user_role, user_refresh_token");               
            else if (type == typeof(User))
                this.queryBuilder.AddSelect(@"user_id, user_email, user_name, user_role, user_role_id, user_active");
            else
                throw new ArgumentException("Type can only be of types 'JwtUser' or 'User'!");

            this.queryBuilder.SetFrom("reg_users");
            this.queryBuilder.AddWhere("user_email = @email");
            var query = this.queryBuilder.BuildQuery();

            var parameters = new Dictionary<string, object>
            {
                {"@email", email}
            };

            var user = _userRepository.GetUser(query, parameters);

            return _mapper.Map<T>(user);
        }

        public User GetUserById(int id, bool isActive = true)
        {
            this.queryBuilder.AddSelect(@"u.user_id, u.user_email, u.user_name, u.user_role, s.user_land, s.user_woonplaats, s.user_opleiding_id, 
                    s.user_op_niveau, s.user_taal, s.user_breedtegraad, s.user_lengtegraad, u.user_active,  
                    l.land_naam, t.talen_naam, o.opl_naam, op.opn_naam");

            this.queryBuilder.SetFrom("reg_users u");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_user_studenten s ON u.user_id = s.user_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_landen l ON s.user_land = l.land_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_talen t ON s.user_taal = t.talen_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_opleidingen o ON s.user_opleiding_id = o.opl_id");
            this.queryBuilder.AddJoinLine("LEFT JOIN reg_opleidingsniveau op ON s.user_op_niveau = op.opn_id");

            this.queryBuilder.AddWhere("u.user_id = @id");

            if (isActive)
                this.queryBuilder.AddWhere("u.user_active = 1");

            string query = this.queryBuilder.BuildQuery();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            var dbUser = _userRepository.GetUser(query, parameters);
            User user = _mapper.Map<User>(dbUser);

            if (user is null)
                return null;

            user.Reviews = this.reviewReadService.GetListReviewIdsForUser(user.Id);
            user.FavoriteVacancies = this.vacancyReadService.GetListVacancyIdsForUser(user.Id);

            return user;
        }

        public JwtUser Login(string password, string email = null, int? userid = null)
        {
            if(string.IsNullOrWhiteSpace(email) && userid == null)
                throw new ArgumentNullException("email OR userid is required!");

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            this.queryBuilder.AddSelect(@"*");
            this.queryBuilder.SetFrom("reg_users");

            if(email != null)
            {
                this.queryBuilder.AddWhere("user_email = @email");
                parameters.Add("@email", email);
            }

            else if(userid != null)
            {
                this.queryBuilder.AddWhere("user_id = @id");
                parameters.Add("@id", userid);
            }

            this.queryBuilder.AddWhere("user_account_type = @type");
            var query = this.queryBuilder.BuildQuery();

            parameters.Add("@type", (int)UserAccountType.ApiUser);

            var user = _userRepository.GetUser(query, parameters);

            if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.user_password))
                return _mapper.Map<JwtUser>(user);
               
            return null;
        }

        public JwtUser GetUserByRefreshToken(string refreshToken)
        {
            this.queryBuilder.AddSelect(@"user_id, user_name, user_role, user_active");
            this.queryBuilder.SetFrom("reg_users");
            this.queryBuilder.AddWhere("user_refresh_token = @token");
            var query = this.queryBuilder.BuildQuery();

            var parameters = new Dictionary<string, object>
            {
                {"@token", refreshToken}
            };

            var user = _userRepository.GetUser(query, parameters);

            return _mapper.Map<JwtUser>(user);
        }

        private void AddMajorsFilter(Dictionary<string, object> parameters, string majorIds)
        {
            if (majorIds is null)
                return;

            // The filter allows multiple majors separated by commas. 
            // Therefore, these should be split and put together with an OR statement in SQL.

            // Split ids
            List<string> splitIds = majorIds.Split(',').ToList();
            List<int> ids = this.CreateIdList(splitIds);
            if (ids.Count <= 0)
                return;

            string educationOr = "(";

            // Put ids together with OR statement
            for (int i = 0; i < ids.Count; i++)
            {
                // Each id should have its own unique parameter
                parameters.Add("@eid" + i.ToString(), ids[i]);
                educationOr += "s.user_opleiding_id = @eid" + i.ToString() + " ";

                if (i != (ids.Count - 1))
                    educationOr += "OR ";
            }

            educationOr += ")";

            this.queryBuilder.AddWhere(educationOr);
        }

        private void AddEducationLevelsFilter(Dictionary<string, object> parameters, string educationLevelIds)
        {
            if (educationLevelIds is null)
                return;

            // The filter allows multiple education levels separated by commas. 
            // Therefore, these should be split and put together with an OR statement in SQL.

            // Split ids
            List<string> splitIds = educationLevelIds.Split(',').ToList();
            List<int> ids = this.CreateIdList(splitIds);
            if (ids.Count <= 0)
                return;

            string educationOr = "(";
            
            // Put ids together with OR statement
            for (int i = 0; i < ids.Count; i++)
            {
                // Each id should have its own unique parameter
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
                bool success = Int32.TryParse(splitId, out int id);
                if (success && id > 0)
                {
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

                    // Select users within the range. The formula is called a haversine formula.
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
            parameters.Add("@countryName", countryName);
        }

        private void AddCityFilter(Dictionary<string, object> parameters, string cityName)
        {
            this.queryBuilder.AddWhere("s.user_woonplaats = @cityName");
            parameters.Add("@cityName", cityName);
        }

        public UserEmailVerification GetUserEmailVerificationByUserId(int userId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", userId);
            string query = "SELECT * FROM reg_user_verification WHERE ver_user_id = @id";

            return _mapper.Map<UserEmailVerification>(_userRepository.GetUserVerification(query, parameters));
        }

        public UserEmailVerification GetUserEmailVerificationByToken(string token)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@token", token);
            string query = "SELECT * FROM reg_user_verification WHERE ver_token = @token";

            return _mapper.Map<UserEmailVerification>(_userRepository.GetUserVerification(query, parameters));
        }
    }
}
