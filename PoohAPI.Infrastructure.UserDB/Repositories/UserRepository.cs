using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.UserDB.Models;
using System.Collections.Generic;

namespace PoohAPI.Infrastructure.UserDB.Repositories
{
    public class UserRepository : MySQLBaseRepository, IUserRepository
    {
        private IMapper _mapper;

        public UserRepository(IMapper mapper, IMySQLClient client) : base(mapper, client)
        {
            _mapper = mapper;
        }

        public IEnumerable<WPUser> GetAllUsers(string query, Dictionary<string, object> parameters)
        {
            return GetAll<WPUser>(query, parameters);
        }

        public WPUser GetUser(string query, Dictionary<string, object> parameters)
        {
            return GetSingle<WPUser>(query, parameters);
        }

        public int RegisterUser(string query, Dictionary<string, object> parameters)
        {
            return NonQuery(query, parameters);
        }
    }
}
