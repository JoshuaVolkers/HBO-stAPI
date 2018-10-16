using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserReadService
    {
        IEnumerable<User> GetAllUsers(int maxCount, int offset);
        User GetUserById(int id);
        User GetUserByEmail(string email);
        User Login(string email, string password);
    }
}
