using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models.RequestModels
{
    public class RegisterRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public int MajorId { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        [Required]
        public bool AcceptTermsAndConditions { get; set; }
    }
}
