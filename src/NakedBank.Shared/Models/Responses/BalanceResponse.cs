using System;

namespace NakedBank.Shared.Models.Responses
{
    public class BalanceResponse : BaseResponse
    {
        public int BalanceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public int AccountId { get; set; }

        public BalanceResponse() : base() { }
    }
}
