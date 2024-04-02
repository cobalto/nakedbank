using NakedBank.Shared;
using System.ComponentModel.DataAnnotations;

namespace NakedBank.Domain
{
    public class PhoneNumber
    {
        private string _phone;

        public PhoneNumber(string humber)
        {
            if (!new PhoneAttribute().IsValid(humber))
            {
                throw new BusinessException("Invalid Phone Number");
            }

            _phone = humber;
        }

        public override string ToString()
        {
            return _phone.ToString();
        }
    }
}