using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// Contains detailed information about vacancies, inherits BaseVacancy class.
    /// </summary>
    public class Vacancy : BaseVacancy
    {
        public string CompanyWebsite { get; set; }
        public IEnumerable<string> CompanySocialLinks { get; set; }
        public DateTime ClosingDate { get; set; }
        public int ViewCount { get; set; }
        public IEnumerable<BaseVacancy> RelatedVacatures { get; set; }
    }
}
