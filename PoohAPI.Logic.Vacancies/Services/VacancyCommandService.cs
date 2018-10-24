using AutoMapper;
using PoohAPI.Infrastructure.VacancyDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Vacancies.Services
{
    class VacancyCommandService : IVacancyCommandService
    {
        private readonly IVacancyRepository vacancyRepository;
        private readonly IMapper mapper;
        private readonly IQueryBuilder queryBuilder;

        public VacancyCommandService(IVacancyRepository vacancyRepository, IMapper mapper, IQueryBuilder queryBuilder)
        {
            this.vacancyRepository = vacancyRepository;
            this.mapper = mapper;
            this.queryBuilder = queryBuilder;
        }

        public void AddFavourite(int userid, int vacancyid)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            string query = @"INSERT INTO reg_vacatures_favoriet (vf_vacature_id, vf_user_id)    
                             VALUES (@userid, @vacancyid)";

            parameters.Add("@userid", userid);
            parameters.Add("@vacancyid", vacancyid);


            vacancyRepository.AddFavouriteVacanacy(query, parameters);
        }

        public void DeleteFavourite(int userid, int vacancyid)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            string query = @"DELETE FROM reg_vacatures_favoriet   
                             WHERE vf_user_id = @userid
                             AND vf_vacature_id = @vacancyid";

            parameters.Add("@userid", userid);
            parameters.Add("@vacancyid", vacancyid);


            vacancyRepository.DeleteFavouriteVacanacy(query, parameters);
        }
    }
}
