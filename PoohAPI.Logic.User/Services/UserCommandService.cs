using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;

namespace PoohAPI.Logic.Users.Services
{
    public class UserCommandService : IUserCommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserCommandService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
    }
}
