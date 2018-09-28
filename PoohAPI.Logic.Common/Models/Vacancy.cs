using System;
using System.Collections.Generic;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Logic.Common.Models
{
    /// <summary>
    /// Contains detailed information about vacancies, inherits BaseVacancy class.
    /// </summary>
    public class Vacancy : BaseVacancy
    {
        public DateTime ClosingDate { get; set; }
        public int ViewCount { get; set; }
        public IEnumerable<BaseVacancy> RelatedVacatures { get; set; }
    }
}
