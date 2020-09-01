using NakedBank.Shared.Models.Responses;

namespace NakedBank.Front.Models
{
    public class User
    {
        public ProfileResponse Profile { get; set; }
        public AuthResponse Authorization { get; set; }

    }
}