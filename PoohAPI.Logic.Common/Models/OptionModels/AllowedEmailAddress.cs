using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models.OptionModels
{
    /// <summary>
    /// Allowed emailaddresses
    /// </summary>
    public class AllowedEmailAddress
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public int EducationalInstitutionId { get; set; }
    }
}
