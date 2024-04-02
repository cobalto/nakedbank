using NakedBank.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NakedBank.Application.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts(int userId);
        Task<Account> GetAccount(int userId, int accountId);
        Task UpdateBalance(int userId, int accountId, decimal balance);
        Task<IEnumerable<Balance>> GetBalances(int userId, int accountId, int days);
        Task<IEnumerable<Transaction>> GetTransactions(int userId, int accountId, int days);
        Task ExecuteBalanceUpdate();
    }
}
