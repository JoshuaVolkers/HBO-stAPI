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
        IEnumerable<T> GetAll<T>(string query);
        void NonQuery(string query);
    }
}
