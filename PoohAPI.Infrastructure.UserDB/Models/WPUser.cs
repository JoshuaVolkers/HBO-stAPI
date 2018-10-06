using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.UserDB.Models
{
    public class WPUser
    {
        public int ID { get; set; }
        public string user_login { get; set; }
        public string user_nicename { get; set; }
        public string user_email { get; set; }
        public DateTime user_registered { get; set; }
        public int user_status { get; set; }
        public string display_name { get; set; }
    }
}
