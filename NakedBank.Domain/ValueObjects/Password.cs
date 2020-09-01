using NakedBank.Shared;
using System;

namespace NakedBank.Domain
{
    public sealed class Password
    {
        private Hash _hash;

        public Hash Hash
        {
            get
            {
                return _hash;
            }
        }

        public Password(Hash hash)
        {
            _hash = hash;
        }

        public Password(string password)
        {
            if (!CheckIfStrongPassword(password))
            {
                throw new BusinessException("Invalid Password");
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);

                _hash = new Hash(BitConverter.ToString(hash).Replace("-", String.Empty));
            }
        }

        private bool CheckIfStrongPassword(string password)
        {
            // Not that strong for now
            return password.Length > 11 ? true : false;
        }

        public bool IsEquals(Password pass)
        {
            return pass.Hash.ToString() == _hash.ToString() ? true : false;
        }

        public override string ToString()
        {
            // Ask for a hash
            return "*********************";
        }
    }
}