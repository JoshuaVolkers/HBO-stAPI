using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.UserDB.Models
{
    public class DBUser
    {
        public int user_id { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
        public string user_name { get; set; }
        public int user_role { get; set; }
        public int user_role_id { get; set; }
        public int user_active { get; set; }
        public int user_land { get; set; }
        public string land_naam { get; set; }
        public string user_woonplaats { get; set; }
        public int user_opleiding_id { get; set; }
        public string opl_naam { get; set; }
        public int user_op_niveau { get; set; }
        public string opn_naam { get; set; }
        public int user_taal { get; set; }
        public string talen_naam { get; set; }
        public int user_social_account { get; set; }
        public decimal user_breedtegraad { get; set; }
        public decimal user_lengtegraad { get; set; }
    }
}
