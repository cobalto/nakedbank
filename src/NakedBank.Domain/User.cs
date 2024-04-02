using System;

namespace NakedBank.Domain
{
    public sealed class User : BaseDomain
    {
        public int UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Login Login { get; private set; }
        public Password Password { get; private set; }
        public Email EmailAddress { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public DateTime LastAccessAt { get; private set; }

        private User() : base() { }

        public User(int userId, string firstName, string lastName,
            Login login, Password password, Email emailAddress,
            PhoneNumber phoneNumber, DateTime lastAccessAt) : base()
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            Password = password;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            LastAccessAt = lastAccessAt;
        }
    }
}
