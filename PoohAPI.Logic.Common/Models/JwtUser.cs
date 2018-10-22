using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Enums;

namespace PoohAPI.Logic.Common.Models
{
    public class JwtUser
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public UserRole Role { get; set; }
        public string RefreshToken { get; set; }
    }
}
