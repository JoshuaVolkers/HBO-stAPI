using System.Collections.Generic;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Logic.Common.Models
{
    /// <summary>
    /// Class with all information of the company, inherits BaseCompany class.
    /// </summary>
    public class Company : BaseCompany
    {
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string Website { get; set; }
        public IEnumerable<string> SocialLinks { get; set; }
    }
}
