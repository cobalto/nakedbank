using System;

namespace NakedBank.Domain
{
    public class Balance : BaseDomain
    {
        public int BalanceId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int AccountId { get; private set; }

        private Balance() : base() { }

        public Balance(int balanceId, decimal amount,
            DateTime timestamp, int accountId)
        {
            BalanceId = balanceId;
            Amount = amount;
            Timestamp = timestamp;
            AccountId = accountId;
        }
    }
}
