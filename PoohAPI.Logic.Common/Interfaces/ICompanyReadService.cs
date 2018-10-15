using PoohAPI.Logic.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface ICompanyReadService
    {
        IEnumerable<Company> GetListCompanies(int maxCount, int offset);
        Company GetCompanyById(int id);
    }
}
