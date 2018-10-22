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

        public int RegisterUser(string query, Dictionary<string, object> parameters)
        {
            return NonQuery(query, parameters);
        }

        public DBUser GetUser(string query, Dictionary<string, object> parameters)
        {
            return GetSingle<DBUser>(query, parameters);
        }

        public void UpdateUser(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public void DeleteUser(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public IEnumerable<DBUser> GetAllUsers(string query, Dictionary<string, object> parameters)
        {
            return GetAll<DBUser>(query, parameters);
        }
    }
}
