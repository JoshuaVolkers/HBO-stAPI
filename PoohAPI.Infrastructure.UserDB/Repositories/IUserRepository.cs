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
        IEnumerable<WPUser> GetAllUsers(string query, Dictionary<string, object> parameters);
        WPUser GetUser(string query, Dictionary<string, object> parameters);
        int RegisterUser(string query, Dictionary<string, object> parameters);
    }
}
