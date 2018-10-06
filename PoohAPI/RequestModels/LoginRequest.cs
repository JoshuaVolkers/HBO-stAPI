using System.ComponentModel.DataAnnotations;

namespace PoohAPI.RequestModels
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
