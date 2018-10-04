using AutoMapper;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.UserDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.UserDB.Repositories
{
    public class UserRepository : MySQLBaseRepository, IUserRepository
    {
        private IMapper _mapper;
        private IMySQLBaseRepository _mySqlBaseRepository;

        public UserRepository(IMapper mapper, IMySQLBaseRepository mySqlBaseRepository)
        {
            _mapper = mapper;
            _mySqlBaseRepository = mySqlBaseRepository;
        }

        public IEnumerable<WPUser> GetAllUsers(int maxCount, int offset)
        {
            return null;
        }

        public WPUser GetUserById(int id)
        {
            return null;
        }
    }
}
