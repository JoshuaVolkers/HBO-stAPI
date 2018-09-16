using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    /// <summary>
    /// BaseUser that contains the same information as the wp_users table in WordPress.
    /// </summary>
    public class BaseUser
    {
        public int Id { get; set; }
        public string NiceName { get; set; }
        public string EmailAddress { get; set; }
        public string Url { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ActivationKey { get; set; }
    }
}
