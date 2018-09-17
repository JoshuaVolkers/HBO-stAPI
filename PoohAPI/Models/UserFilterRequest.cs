using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// User request model that can be used to filter out specific users.
    /// </summary>
    public class UserFilterRequest
    {
        public IEnumerable<string> EducationalAttainment { get; set; }
        public IEnumerable<string> Educations { get; set; }
        public Location Location { get; set; }
        public IEnumerable<string> PreferredLanguages { get; set; }
    }
}
