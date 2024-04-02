using System;

namespace NakedBank.Shared.Models.Responses
{
    public class ProfileResponse : BaseResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastAccessAt { get; set; }

        public ProfileResponse() : base() { }
    }
}
