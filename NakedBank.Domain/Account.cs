namespace NakedBank.Domain
{
    public class Account : BaseDomain
    {
        public int AccountId { get; set; }
        public int BranchId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; } = 0;
        public int UserId { get; set; }

        private Account() : base() { }

        public Account(int accountId, int branchId, string accountNumber,
            decimal balance, int userId) : base()
        {
            AccountId = accountId;
            BranchId = branchId;
            AccountNumber = accountNumber;
            Balance = balance;
            UserId = userId;
        }

        private bool IsValidBarCode(string barCode)
        {
            // FEBRABAN high level validation
            return barCode.Length == 44;
        }

        public bool CanExecutePayment(decimal amount, string barCode)
        {
            // TODO: Apply overdraft rules, maybe

            if (amount > (this.Balance * 1.1m))
            {
                Errors.Add(new Shared.BusinessError(nameof(this.Balance), "Not enough founds"));
            }

            // Some special rule for Payment

            if (!IsValidBarCode(barCode))
            {
                Errors.Add(new Shared.BusinessError(nameof(barCode), "Problem validating barcode"));
            }

            if (!HasErrors)
                Balance -= amount;

            return !this.HasErrors;
        }

        public bool CanExecuteDeposit(decimal amount)
        {
            // Some special rule for Deposit

            if (!HasErrors)
                Balance += amount;

            return !this.HasErrors;
        }

        public bool CanExecuteWithdraw(decimal amount)
        {
            // TODO: Apply overdraft rules, maybe

            if (amount > (this.Balance * 1.05m))
            {
                Errors.Add(new Shared.BusinessError(nameof(this.Balance), "Not enough founds"));
            }

            // Some special rule for Withdraw

            if(!HasErrors)
                Balance -= amount;

            return !this.HasErrors;
        }
    }
}
