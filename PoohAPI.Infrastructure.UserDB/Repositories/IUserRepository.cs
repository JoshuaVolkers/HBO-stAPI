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
        IEnumerable<DBUser> GetAllUsers(string query, Dictionary<string, object> parameters);
        DBUser GetUser(string query, Dictionary<string, object> parameters);
        DBUserEmailVerification GetUserVerification(string query, Dictionary<string, object> parameters);
        int UpdateDelete(string query, Dictionary<string, object> parameters);
        string GetUserPassword(string query, Dictionary<string, object> parameters);
    }
}
