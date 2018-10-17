using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.UserDB.Models
{
    public class WPUser
    {
        public int user_id { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
        public string user_name { get; set; }    
        public int user_role { get; set; }
        public int user_role_id { get; set; }
        public int user_active { get; set; }
    }
}
