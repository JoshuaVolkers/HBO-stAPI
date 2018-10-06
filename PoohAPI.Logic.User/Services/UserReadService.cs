using AutoMapper;
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
            var users = _userRepository.GetAllUsers(maxCount, offset);
            return _mapper.Map<IEnumerable<Common.Models.User>>(users);
        }

        public Common.Models.User GetUserById(int id)
        {
            var query = string.Format("SELECT * FROM users WHERE user.id = {0}", id);
            var user = _userRepository.GetUserById(query);
            
            return _mapper.Map<Common.Models.User>(user);
        }
    }
}
