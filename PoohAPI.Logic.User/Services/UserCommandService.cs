using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.InputModels;
using System.Collections.Generic;

namespace PoohAPI.Logic.Users.Services
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMapAPIReadService mapAPIReadService;
        private readonly IQueryBuilder queryBuilder;
        private readonly IUserReadService userReadService;

        public UserCommandService(IUserRepository userRepository, IMapper mapper, 
            IMapAPIReadService mapAPIReadService, IQueryBuilder queryBuilder, IUserReadService userReadService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            this.mapAPIReadService = mapAPIReadService;
            this.queryBuilder = queryBuilder;
            this.userReadService = userReadService;
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

            _userRepository.DeleteUser(query, parameters);
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

            _userRepository.UpdateUser(query, parameters);
            

            return this.userReadService.GetUserById(userInput.Id);
        }
    }
}
