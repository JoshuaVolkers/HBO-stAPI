using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Infrastructure.UserDB.Models
{
    public class DBUserEmailVerification
    {
        public int ver_id { get; set; }
        public int ver_user_id { get; set; }
        public string ver_token { get; set; }
        public DateTime ver_expiration { get; set; }
    }
}
