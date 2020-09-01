using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NakedBank.Application.Repositories;
using NakedBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NakedBank.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        // Convert this to a config table in database
        private const decimal ANNUAL_INTEREST_RATE = 1.9m;

        private readonly NakedContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(NakedContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Account>> GetAccounts(int userId)
        {
            var accounts = await _context.Accounts
                                         .Where(acc => acc.UserId == userId)
                                         .ToListAsync();

            return accounts.Select(acc => _mapper.Map<Account>(acc));
        }

        public async Task<Account> GetAccount(int userId, int accountId)
        {
            var account = await _context.Accounts
                                        .Where(acc => acc.UserId == userId)
                                        .Where(acc => acc.AccountId == accountId)
                                        .FirstOrDefaultAsync();

            return _mapper.Map<Account>(account);
        }

        public async Task<IEnumerable<Balance>> GetBalances(int userId, int accountId, int days)
        {
            var minimumDate = DateTime.UtcNow.AddDays(-days);

            var balances = _context.Balances.Where(b => b.AccountId == accountId)
                                            .Where(b => b.Account.UserId == userId);

            balances = balances.Where(b => b.Timestamp > minimumDate);

            return await balances.Select(balance => _mapper.Map<Balance>(balance)).ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(int userId, int accountId, int days)
        {
            var minimumDate = DateTime.UtcNow.AddDays(-days);

            var transactions = _context.Transactions.Where(t => t.AccountId == accountId)
                                                    .Where(t => t.Account.UserId == userId);

            transactions = transactions.Where(b => b.Timestamp > minimumDate);

            return await transactions.Select(transaction => _mapper.Map<Transaction>(transaction)).ToListAsync();
        }

        public async Task UpdateBalance(int userId, int accountId, decimal balance)
        {
            var account = await _context.Accounts
                                        .Where(acc => acc.UserId == userId)
                                        .Where(acc => acc.AccountId == accountId)
                                        .FirstOrDefaultAsync();

            account.Balance = balance;

            await _context.SaveChangesAsync();
        }

        public async Task ExecuteBalanceUpdate()
        {
            var dailyInterest = ANNUAL_INTEREST_RATE / 365;

            var accounts = await _context.Accounts.ToListAsync();

            foreach(var acc in accounts)
            {
                if (_context.Balances.Where(b => b.Timestamp == DateTime.Today).Any())
                    continue;

                Entities.Balance nb = new Entities.Balance()
                {
                    AccountId = acc.AccountId,
                    Timestamp = DateTime.Today,
                    Amount = acc.Balance
                };

                _context.Balances.Add(nb);

                acc.Balance += acc.Balance * dailyInterest;
            }

            await _context.SaveChangesAsync();
        }
    }
}
