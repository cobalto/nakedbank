using AutoMapper;
using NakedBank.Application.Repositories;
using NakedBank.Infrastructure.Entities;
using NakedBank.Shared.Models;
using System;
using System.Threading.Tasks;

namespace NakedBank.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly NakedContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(NakedContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.Transaction> ExecuteTransaction(TransactionType transactionType, int accountId, decimal amount, decimal newBalance, string barCode = null)
        {
            var newTransaction = new Transaction()
            {
                TransactionType = (int)transactionType,
                AccountId = accountId,
                Amount = amount,
                BalanceAfterTransaction = newBalance,
                Timestamp = DateTime.UtcNow,
                BarCode = barCode
            };

            await _context.Transactions.AddAsync(newTransaction);
            await _context.SaveChangesAsync();

            return _mapper.Map<Domain.Transaction>(newTransaction);
        }
    }
}
