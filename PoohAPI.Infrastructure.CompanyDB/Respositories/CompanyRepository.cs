using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.CompanyDB.Models;

namespace PoohAPI.Infrastructure.CompanyDB.Respositories
{
    public class CompanyRepository : MySQLBaseRepository, ICompanyRepository
    {
        private IMapper _mapper;
        public CompanyRepository(IMapper mapper, IMySQLClient client) : base(mapper, client)
        {
            _mapper = mapper;
        }

        public DBCompany GetCompany(string query)
        {
            return GetSingle<DBCompany>(query);
        }

        public DBCompany GetCompany(string query, Dictionary<string, object> parameters)
        {
            return GetSingle<DBCompany>(query, parameters);
        }

        public IEnumerable<DBCompany> GetListCompanies(string query)
        {
            return GetAll<DBCompany>(query);
        }

        public IEnumerable<DBCompany> GetListCompanies(string query, Dictionary<string, object> parameters)
        {
            return GetAll<DBCompany>(query, parameters);
        }
    }
}
