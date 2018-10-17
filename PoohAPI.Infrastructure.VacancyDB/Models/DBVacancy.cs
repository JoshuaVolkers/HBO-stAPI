using PoohAPI.Logic.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.VacancyDB.Models
{
    public class DBVacancy
    {
        public int vacature_id { get; set; }
        public int vacature_bedrijf_id { get; set; }
        public int vacature_user_id { get; set; }
        public string vacature_titel { get; set; }
        public string vacature_plaats { get; set; }
        public DateTime vacature_datum_plaatsing { get; set; }
        public DateTime vacature_datum_verlopen { get; set; }
        public string vacature_text { get; set; }
        public string vacature_link { get; set; }
        public int vacature_actief { get; set; }
        public decimal vacature_breedtegraad { get; set; }
        public decimal vacature_lengtegraad { get; set; }
        public string talen_naam { get; set; }
        public string opn_naam { get; set; }
        public string opleidingen { get; set; }
        public int bedrijf_vestiging_land { get; set; }
        public string bedrijf_vestiging_plaats { get; set; }
        public string bedrijf_vestiging_straat { get; set; }
        public string bedrijf_vestiging_huisnr { get; set; }
        public string bedrijf_vestiging_toev { get; set; }
        public string bedrijf_vestiging_postcode { get; set; }
        public string land_naam { get; set; }
        public double distance { get; set; }
        public string stagesoort { get; set; }
        public IEnumerable<IntershipType> stagesoortenumlist { get; set; }
    }
}
