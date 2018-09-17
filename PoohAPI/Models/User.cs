using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// User model that contains userprofile information.
    /// </summary>
    public class User : BaseUser
    {
        /// <summary>
        /// For Dutch speakers, dit is het opleidingsniveau.
        /// </summary>
        public string EducationalAttainment { get; set; }
        public IEnumerable<string> Educations { get; set; }
        public Location Location { get; set; }
        public IEnumerable<BaseVacancy> FavoriteVacancies { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<string> PreferredLanguages { get; set; }
    }
}

//TODO:
// Endpoint maken om alle users op te halen, is voor admins.
// Language filters voor companies en vacancies.
// Review moet gedelete, aangepast kunnen worden binnen een bepaalde tijd.