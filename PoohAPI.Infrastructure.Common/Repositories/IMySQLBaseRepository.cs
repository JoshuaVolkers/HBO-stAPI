using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.Common.Repositories
{
    public interface IMySQLBaseRepository
    {
        T GetSingle<T>(string query);
        T GetSingle<T>(string query, Dictionary<string, object> parameters);
        IEnumerable<T> GetAll<T>(string query);
        IEnumerable<T> GetAll<T>(string query, Dictionary<string, object> parameters);
        int NonQuery(string query, Dictionary<string, object> parameters);
    }
}
