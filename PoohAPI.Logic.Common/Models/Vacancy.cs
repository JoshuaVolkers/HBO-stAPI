using System;
using System.Collections.Generic;
using PoohAPI.Logic.Common.Enums;
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
        public string Education { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public string Language { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string City { get; set; }
        public string Link { get; set; }
        public string EducationalAttainment { get; set; }
        public IntershipType IntershipType { get; set; }
        public Company Company { get; set; }
    }
}
