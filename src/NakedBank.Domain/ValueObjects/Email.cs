using NakedBank.Shared;
using System.ComponentModel.DataAnnotations;

namespace NakedBank.Domain
{
    public sealed class Email
    {
        private string _email;

        public Email(string email)
        {
            if (!new EmailAddressAttribute().IsValid(email))
            {
                throw new BusinessException("Invalid Email Address");
            }

            _email = email;
        }

        public override string ToString()
        {
            return _email.ToString();
        }
    }
}