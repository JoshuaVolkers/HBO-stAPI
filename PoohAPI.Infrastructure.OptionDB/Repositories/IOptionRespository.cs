using PoohAPI.Infrastructure.OptionDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.OptionDB.Repositories
{
    public interface IOptionRepository
    {
        IEnumerable<DBMajor> GetAllMajors(string query, Dictionary<string, object> parameters);
        IEnumerable<DBEducationLevel> GetAllEducationLevels(string query, Dictionary<string, object> parameters);
    }
}
