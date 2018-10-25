using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.OptionDB.Models
{
    public class DBAllowedEmailAddress
    {
        public int se_id { get; set; }
        public string se_domein { get; set; }
        public int se_onderwijsinstelling_id { get; set; }
    }
}
