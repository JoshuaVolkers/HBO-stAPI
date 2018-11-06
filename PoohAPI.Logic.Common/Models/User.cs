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
        public int EducationLevelId { get; set; }
        public string EducationLevel { get; set; }
        public int MajorId { get; set; }
        public string Major { get; set; }
        public BaseLocation Location { get; set; }
        public IEnumerable<int> FavoriteVacancies { get; set; }
        public IEnumerable<int> Reviews { get; set; }
        public int PreferredLanguageId { get; set; }
        public string PreferredLanguage { get; set; }
    }
}