using NakedBank.Shared.Models;
using NakedBank.Shared.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NakedBank.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponse>> GetAccounts(int userId);
        Task<IEnumerable<BalanceResponse>> GetBalances(int userId, int accountId, int days);
        Task<IEnumerable<TransactionResponse>> GetTransactions(int userId, int accountId, int days);
        Task<TransactionResponse> ExecuteTransaction(TransactionType type, int userId, int accountId, decimal Amount, string barCode = null);
        Task ExecuteBalanceUpdate();
    }
}
