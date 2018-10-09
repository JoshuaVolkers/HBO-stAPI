using System;
using System.Collections.Generic;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Logic.Common.Models
{
    /// <summary>
    /// Contains detailed information about vacancies.
    /// </summary>
    public class Vacancy
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<string> Education { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public string Language { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}
