using AutoMapper;
using PoohAPI.Infrastructure.VacancyDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Classes;

namespace PoohAPI.Logic.Vacancies.Services
{
    class VacancyCommandService : IVacancyCommandService
    {
        private readonly IVacancyRepository vacancyRepository;
        private readonly IMapper mapper;
        private readonly IQueryBuilder queryBuilder;

        public VacancyCommandService(IVacancyRepository vacancyRepository, IMapper mapper)
        {
            this.vacancyRepository = vacancyRepository;
            this.mapper = mapper;
            this.queryBuilder = new QueryBuilder();
        }
        
        /// <summary>
        /// Adding a vacancy as favorite
        /// </summary>
        /// <param name="userid">id of the user that is adding the vacancy as favorite</param>
        /// <param name="vacancyid">id of the vacancy that is being added as a favorite</param>
        public void AddFavourite(int userid, int vacancyid)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@userid", userid},
                { "@vacancyid", vacancyid}
            };

            string query = @"INSERT INTO reg_vacatures_favoriet (vf_vacature_id, vf_user_id)    
                             VALUES (@vacancyid, @userid)";

            vacancyRepository.AddFavouriteVacanacy(query, parameters);
        }

        /// <summary>
        /// Deleting a favourite vacancy with from user
        /// </summary>
        /// <param name="userid">id of the user</param>
        /// <param name="vacancyid">id of the vacancy to be unfavorited</param>
        public void DeleteFavourite(int userid, int vacancyid)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@userid", userid},
                { "@vacancyid", vacancyid}
            };

            string query = @"DELETE FROM reg_vacatures_favoriet   
                             WHERE vf_user_id = @userid
                             AND vf_vacature_id = @vacancyid";

            vacancyRepository.DeleteFavouriteVacanacy(query, parameters);
        }
    }
}
