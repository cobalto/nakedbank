using NakedBank.Shared.Models;
using System;

namespace NakedBank.Domain
{
    public class Transaction : BaseDomain
    {
        public Guid TransactionId { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int AccountId { get; private set; }

        private Transaction() : base() { }

        public Transaction(Guid transactionId, TransactionType transactionType,
            decimal amount, DateTime timestamp, int accountId)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
            Amount = amount;
            Timestamp = timestamp;
            AccountId = accountId;
        }
    }
}
