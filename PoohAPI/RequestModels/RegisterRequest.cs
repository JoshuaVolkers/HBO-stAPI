using System.ComponentModel.DataAnnotations;

namespace PoohAPI.RequestModels
{
    public class RegisterRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public bool AcceptTermsAndConditions { get; set; }
    }
}
