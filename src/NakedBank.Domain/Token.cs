using System;

namespace NakedBank.Domain
{
    public sealed class Token : BaseDomain
    {
        public Guid TokenId { get; private set; }
        public int UserId { get; private set; }
        public string TokenString { get; private set; }
        public DateTime IssuedAt { get; private set; }
        public DateTime ValidSince { get; private set; }
        public DateTime ValidUntil { get; private set; }

        private Token() : base() { }

        public Token(int userId, string tokenString, DateTime issuedAt,
            DateTime validSince, DateTime validUntil) : base()
        {
            UserId = userId;
            TokenString = tokenString;
            IssuedAt = issuedAt;
            ValidSince = validSince;
            ValidUntil = validUntil;
        }
    }
}
