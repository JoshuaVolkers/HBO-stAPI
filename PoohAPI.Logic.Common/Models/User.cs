using System.Collections.Generic;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Logic.Common.Models
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
        public IEnumerable<int> FavoriteVacancies { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<string> PreferredLanguages { get; set; }
    }
}