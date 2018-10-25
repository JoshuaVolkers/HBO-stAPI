using AutoMapper;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.OptionDB.Models;
using System.Collections.Generic;

namespace PoohAPI.Infrastructure.OptionDB.Repositories
{
    public class OptionRepository : MySQLBaseRepository, IOptionRepository
    {
        private IMapper _mapper;

        public OptionRepository(IMapper mapper, IMySQLClient client) : base(mapper, client)
        {
            _mapper = mapper;
        }

        public IEnumerable<DBMajor> GetAllMajors(string query, Dictionary<string, object> parameters)
        {
            return GetAll<DBMajor>(query, parameters);
        }        
    }
}
