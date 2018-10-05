using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.UserDB.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.UserDB.Repositories
{
    public class UserRepository : MySQLBaseRepository, IUserRepository
    {
        private IMapper _mapper;

        public UserRepository(IMapper mapper) : base(mapper)
        {
            _mapper = mapper;
        }

        public IEnumerable<WPUser> GetAllUsers(int maxCount, int offset)
        {
            var i = ConfigurationReader.TestValue;          
            return null;
        }

        public WPUser GetUserById(int id)
        {
            return null;
        }
    }
}
