using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordPressPCL;
using WordPressPCL.Models;

namespace PoohAPI.Infrastructure.UserDB.Repositories
{
    public class UserRepository : IUserRepository
    {
        private WordPressClient _client { get; set; }

        public UserRepository()
        {
            _client = new WordPressClient("http://dev.hbo-stagemarkt.nl/wp-json/");
            _client.AuthMethod = AuthMethod.JWT;
            _client.RequestJWToken("", "");
        }

        public async Task<IEnumerable<WPUser>> GetAllUsersAsync()
        {
            CreateClientAsync();
            var list = await _client.Users.GetAll();
            return Mapper.Map<IEnumerable<WPUser>>(list);
        }

        public async Task<WPUser> GetUserByIdAsync(int id)
        {
            CreateClientAsync();
            var user = await _client.Users.GetByID(id);
            return Mapper.Map<WPUser>(user);
        }

        private async void CreateClientAsync()
        {
            if (!await _client.IsValidJWToken())
            {
                _client = new WordPressClient("http://dev.hbo-stagemarkt.nl/wp-json/");
                _client.AuthMethod = AuthMethod.JWT;
                await _client.RequestJWToken("", "");
            }
        }
    }
}
