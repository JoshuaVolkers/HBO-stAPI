using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
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

        public IEnumerable<User> GetAllUsers(int maxCount, int offset)
        {
            var query = string.Format("SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
                                      " FROM reg_users LIMIT {0} OFFSET {1}", maxCount, offset);
            var users = _userRepository.GetAllUsers(query);
            return _mapper.Map<IEnumerable<User>>(users);
        }

        public User GetUserById(int id)
        {
            var query = string.Format("SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
                                      " FROM reg_users WHERE user_id = {0}", id);
            var user = _userRepository.GetUser(query);

            return _mapper.Map<User>(user);
        }

        public User GetUserByEmail(string email)
        {
            var query = string.Format("SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
                                      " FROM reg_users WHERE user_email = {0}", email);
            var user = _userRepository.GetUser(query);

            return _mapper.Map<User>(user);
        }

        public User Login(string login, string password)
        {
            //var encoded = PasswordHasher.Encode(password, user.user_pass);

            var query = string.Format("SELECT * FROM reg_users WHERE user_name = '{0}'", login);
            var user = _userRepository.GetUser(query);
            if (user != null && user.user_password.Equals(password))
                return _mapper.Map<User>(user);

            return null;
        }
    }
}
