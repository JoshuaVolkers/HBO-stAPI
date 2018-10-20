using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models
{
    public class UserEmailVerification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
