using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Users.Services
{
    public class UserReadService : IUserReadService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserReadService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<Common.Models.User> GetAllUsers(int maxCount, int offset)
        {
            var query = string.Format("SELECT * FROM wp_dev_users LIMIT {0} OFFSET {1}", maxCount, offset);
            var users = _userRepository.GetAllUsers(query);
            return _mapper.Map<IEnumerable<Common.Models.User>>(users);
        }

        public Common.Models.User GetUserById(int id)
        {
            var query = string.Format("SELECT * FROM wp_dev_users WHERE ID = {0}", id);
            var user = _userRepository.GetUserById(query);
            
            return _mapper.Map<Common.Models.User>(user);
        }
    }
}
