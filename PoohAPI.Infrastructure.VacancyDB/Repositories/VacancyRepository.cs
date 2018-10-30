using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.VacancyDB.Models;

namespace PoohAPI.Infrastructure.VacancyDB.Repositories
{
    public class VacancyRepository : MySQLBaseRepository, IVacancyRepository
    {
        private IMapper _mapper;
        public VacancyRepository(IMapper mapper, IMySQLClient client) : base(mapper, client)
        {
            _mapper = mapper;
        }

        public void AddFavouriteVacanacy(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public void DeleteFavouriteVacanacy(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public IEnumerable<DBVacancy> GetListVacancies(string query)
        {
            return GetAll<DBVacancy>(query);
        }

        public IEnumerable<DBVacancy> GetListVacancies(string query, Dictionary<string, object> parameters)
        {
            return GetAll<DBVacancy>(query, parameters);
        }

        public IEnumerable<DBVacancyId> GetListVacancyIds(string query, Dictionary<string, object> parameters)
        {
            return GetAll<DBVacancyId>(query, parameters);
        }

        public DBVacancy GetVacancy(string query)
        {
            return GetSingle<DBVacancy>(query);
        }

        public DBVacancy GetVacancy(string query, Dictionary<string, object> parameters)
        {
            return GetSingle<DBVacancy>(query, parameters);
        }
    }
}
