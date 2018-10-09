using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoohAPI.Logic.Users.Helpers;

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

        public IEnumerable<User> GetAllUsers(int maxCount, int offset)
        {
            var query = string.Format("SELECT * FROM wp_dev_users LIMIT {0} OFFSET {1}", maxCount, offset);
            var users = _userRepository.GetAllUsers(query);
            return _mapper.Map<IEnumerable<User>>(users);
        }

        public Common.Models.User GetUserById(int id)
        {
            var query = string.Format("SELECT * FROM wp_dev_users WHERE ID = {0}", id);
            var user = _userRepository.GetUser(query);
            
            return _mapper.Map<User>(user);
        }

        public User Login(string login, string password)
        {
            var query = string.Format("SELECT * FROM wp_dev_users WHERE user_login = '{0}'", login);
            var user = _userRepository.GetUser(query);
            var encoded = PasswordHasher.Encode(password, user.user_pass);
            if (encoded.Equals(user.user_pass))
                return _mapper.Map<User>(user);

            return null;
        }
    }
}
