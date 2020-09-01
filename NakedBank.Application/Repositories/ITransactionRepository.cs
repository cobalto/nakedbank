using NakedBank.Domain;
using NakedBank.Shared.Models;
using System.Threading.Tasks;

namespace NakedBank.Application.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> ExecuteTransaction(TransactionType transactionType, int accountId, decimal amount, decimal newBalance, string barCode = null);
    }
}
