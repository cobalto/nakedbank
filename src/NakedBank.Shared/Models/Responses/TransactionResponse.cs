using System;

namespace NakedBank.Shared.Models.Responses
{
    public class TransactionResponse : BaseResponse
    {
        public Guid TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public int AccountId { get; set; }

        public TransactionResponse() : base() { }
    }
}
