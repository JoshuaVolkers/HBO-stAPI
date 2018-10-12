using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface ICompanyReadService
    {
        IEnumerable<BaseCompany> GetListCompanies(int maxCount, int offset, double? minStars = null,
            double? maxStars = null, string cityName = null, string countryName = null, int? locationRange = null,
            string additionalLocationSearchTerms = null, int? major = null, bool detailedCompanies = false);
        Company GetCompanyById(int id);
    }
}
