namespace NakedBank.Shared.Models.Responses
{
    public class AccountResponse : BaseResponse
    {
        public int AccountId { get; set; }
        public int BranchId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;

        public AccountResponse() : base() { }
    }
}
