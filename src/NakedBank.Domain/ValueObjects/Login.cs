using NakedBank.Shared;
using System.Text.RegularExpressions;

namespace NakedBank.Domain
{
    public sealed class Login
    {
        private string _login;

        public Login(string login)
        {
            if (!new Regex(@"^\d{11}$").IsMatch(login))
            {
                throw new BusinessException("Invalid Login");
            }

            _login = login;
        }

        public override string ToString()
        {
            return _login.ToString();
        }
    }
}