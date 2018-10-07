using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.Common.Repositories;


namespace PoohAPI.Infrastructure.CompanyDB.Respositories
{
    public class CompanyRepository : MySQLBaseRepository, ICompanyRepository
    {
        private IMapper _mapper;
        public CompanyRepository(IMapper mapper, IMySQLClient client) : base(mapper, client)
        {
            _mapper = mapper;
        }
    }
}
