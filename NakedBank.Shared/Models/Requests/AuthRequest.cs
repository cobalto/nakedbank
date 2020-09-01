using System.ComponentModel.DataAnnotations;

namespace NakedBank.Shared.Models.Requests
{
    public class AuthRequest
    {
        [Required, StringLength(11)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
