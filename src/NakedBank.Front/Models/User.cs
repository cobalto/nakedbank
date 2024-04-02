using NakedBank.Shared.Models.Responses;
using System.Collections.Generic;

namespace NakedBank.Front.Models
{
    public class User
    {
        public ProfileResponse Profile { get; set; }
        public AuthResponse Authorization { get; set; }
        public IEnumerable<AccountResponse> Accounts { get; set; }
    }
}