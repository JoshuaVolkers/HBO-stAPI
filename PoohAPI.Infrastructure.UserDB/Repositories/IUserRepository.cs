using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Infrastructure.UserDB.Models;

namespace PoohAPI.Infrastructure.UserDB.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<WPUser>> GetAllUsersAsync();
        Task<WPUser> GetUserByIdAsync(int id);
    }
}
