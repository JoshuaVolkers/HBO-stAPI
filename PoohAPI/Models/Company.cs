using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
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
