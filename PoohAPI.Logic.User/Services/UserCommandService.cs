using System.Collections.Generic;
using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.InputModels;

namespace PoohAPI.Logic.Users.Services
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserRepository userRepository;       
        private readonly IMapper mapper;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly IQueryBuilder queryBuilder;
        private readonly IUserReadService userReadService;

        public UserCommandService(IUserRepository userRepository, IMapper mapper, 
            IMapAPIReadService mapAPIReadService, IQueryBuilder queryBuilder, IUserReadService userReadService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.mapAPIReadService = mapAPIReadService;
            this.queryBuilder = queryBuilder;
            this.userReadService = userReadService;
        }

        public User RegisterUser(string login, string email, UserAccountType accountType, string password = null)
        {
            //SELECT LAST_INSERT_ID() returns the primary key of the created record.
            var query = string.Format("INSERT INTO reg_users (user_email, user_password, user_name,  user_role, user_role_id, user_account_type) " +
                                      " VALUES(@user_email, @user_password, @user_name, @user_role, @user_role_id, @user_account_type);" +
                                      "SELECT LAST_INSERT_ID()");
            var parameters = new Dictionary<string, object>();
            parameters.Add("@user_email", email);          
            parameters.Add("@user_name", login);
            parameters.Add("@user_role", 0);
            parameters.Add("@user_role_id", 0);
            parameters.Add("@user_account_type", (int)accountType);

            if (!string.IsNullOrEmpty(password))
            {
                var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 10);
                parameters.Add("@user_password", hashedPassword);
            }
            else
            {
                parameters.Add("@user_password", null);
            }

            //TODO: TEST IF THE ABOVE IF ELSE WORKS!
            //Implement email address check, retrieve from fake optionsservice for now.
            //Implement a "foreignStudent" boolean for the register request. Also add a field for the required legal document (school pas o.i.d.).
                
            var createdUserId = this.userRepository.RegisterUser(query, parameters);

            return this.mapper.Map<User>(this.userReadService.GetUserById(createdUserId));
        }
       
        

        public void DeleteUser(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@id", id);

            string query = @"UPDATE reg_reviews SET review_student_id = 0 WHERE review_student_id = @id;
                             DELETE FROM reg_vacatures_favoriet WHERE vf_user_id = @id;
                             DELETE FROM reg_user_studenten WHERE user_id = @id;
                             DELETE FROM reg_users WHERE user_id = @id;
                            ";

            this.userRepository.DeleteUser(query, parameters);
        }

        public User UpdateUser(UserUpdateInput userInput)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            this.queryBuilder.Clear();
            this.queryBuilder.SetUpdate("reg_user_studenten");
            this.queryBuilder.AddUpdateSet(@"user_land = @countryId, user_woonplaats = @cityName, user_opleiding_id = @educationId, 
                                 user_op_niveau = @educationLevelId, user_taal = @languageId");
            this.queryBuilder.AddWhere("user_id = @id");

            parameters.Add("@countryId", userInput.CountryId);
            parameters.Add("@cityName", userInput.City);
            parameters.Add("@educationId", userInput.EducationId);
            parameters.Add("@educationLevelId", userInput.EducationalAttainmentId);
            parameters.Add("@languageId", userInput.PreferredLanguageId);
            parameters.Add("@id", userInput.Id);

            Coordinates coordinates = this.mapAPIReadService.GetMapCoordinates(userInput.City, null, userInput.AdditionalLocationIdentifier);

            if (coordinates is Coordinates)
            {
                this.queryBuilder.AddUpdateSet("user_breedtegraad = @latitude");
                this.queryBuilder.AddUpdateSet("user_lengtegraad = @longitude");
                parameters.Add("@latitude", coordinates.Latitude);
                parameters.Add("@longitude", coordinates.Longitude);
            }

            string query = this.queryBuilder.BuildUpdate();
            this.queryBuilder.Clear();

            this.userRepository.UpdateUser(query, parameters);
            
            return this.userReadService.GetUserById(userInput.Id);
        }
    }
}
