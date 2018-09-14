using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    public class Vacature : BaseVacature
    {
        public string CompanyWebsite { get; set; }
        public IEnumerable<string> CompanySocialLinks { get; set; }
        public DateTime ClosingDate { get; set; }
        public int ViewCount { get; set; }
        public IEnumerable<BaseVacature> RelatedVacatures { get; set; }
    }
}
