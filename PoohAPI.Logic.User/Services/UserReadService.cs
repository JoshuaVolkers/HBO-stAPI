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

        public async Task<IEnumerable<Common.Models.User>> GetAllUsersAsync(int maxCount, int offset)
        {
            var users = await _userRepository.GetAllUsersAsync(maxCount, offset);
            return _mapper.Map<IEnumerable<Common.Models.User>>(users);
        }

        public async Task<Common.Models.User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<Common.Models.User>(user);
        }
    }
}
