using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.CompanyDB.Models
{
    public class DBCompany
    {
        public int bedrijf_id { get; set; }
        public string bedrijf_handelsnaam { get; set; }
        public string bedrijf_vestiging_straat { get; set; }
        public string bedrijf_vestiging_huisnr { get; set; }
        public string bedrijf_vestiging_toev { get; set; }
        public string bedrijf_vestiging_postcode { get; set; }
        public string bedrijf_vestiging_plaats { get; set; }
        public int bedrijf_vestiging_land { get; set; }
        public string land_naam { get; set; }
        public string bedrijf_contactpersoon_email { get; set; }
        public string bedrijf_contactpersoon_telefoon { get; set; }
        public string bedrijf_website { get; set; }
        public string bedrijf_logo { get; set; }
        public string bedrijf_social_linkedin { get; set; }
        public string bedrijf_beschrijving { get; set; }
        public decimal bedrijf_breedtegraad { get; set; }
        public decimal bedrijf_lengtegraad { get; set; }
        public decimal average_reviews { get; set; }
        public double distance { get; set; }
    }
}
