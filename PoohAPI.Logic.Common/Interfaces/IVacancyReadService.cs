using PoohAPI.Logic.Common.Enums;
using PoohAPI.Logic.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IVacancyReadService
    {
        IEnumerable<Vacancy> GetListVacancies(int maxCount = 5, int offset = 0, string additionalLocationSearchTerms = null,
string education = null, string educationalAttainment = null, IntershipType? intershipType = null, string languages = null, string cityName = null, string countryName = null, int? locationRange = null);

        Vacancy GetVacancyById(int id);
    }
}
