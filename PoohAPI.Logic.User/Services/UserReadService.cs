using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Helpers;

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
            var query = "SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
                                      " FROM reg_users LIMIT @maxCount OFFSET @offset";

            var parameters = new Dictionary<string, object>
            {
                { "@maxCount", maxCount },
                { "@offset", offset }
            };

            var users = _userRepository.GetAllUsers(query, parameters);
            return _mapper.Map<IEnumerable<User>>(users);
        }

        public User GetUserById(int id)
        {
            var query = "SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
                        " FROM reg_users WHERE user_id = @id";
            var parameters = new Dictionary<string, object>
            {
                { "@id", id }
            };

            var user = _userRepository.GetUser(query, parameters);

            return _mapper.Map<User>(user);
        }

        public User GetUserByEmail(string email)
        {
            var query = "SELECT user_id, user_email, user_name, user_role, user_role_id, user_active" +
                                      " FROM reg_users WHERE user_email = @email";
            var parameters = new Dictionary<string, object>
            {
                { "@email", email }
            };

            var user = _userRepository.GetUser(query, parameters);

            return _mapper.Map<User>(user);
        }

        public User Login(string email, string password)
        {
            var query = "SELECT * FROM reg_users WHERE user_email = @email";
            var parameters = new Dictionary<string, object>
            {
                { "@email", email }
            };

            var user = _userRepository.GetUser(query, parameters);

            if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.user_password))
                return _mapper.Map<User>(user);

            return null;
        }
    }
}
