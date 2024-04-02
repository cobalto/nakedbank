using AutoMapper;
using NakedBank.Application.Interfaces;
using NakedBank.Application.Repositories;
using NakedBank.Shared.Models;
using NakedBank.Shared.Models.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NakedBank.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountResponse>> GetAccounts(int userId)
        {
            var accounts = await _accountRepository.GetAccounts(userId);

            List<AccountResponse> response = accounts.Select(acc => _mapper.Map<AccountResponse>(acc)).ToList();

            return response;
        }

        public async Task<IEnumerable<BalanceResponse>> GetBalances(int userId, int accountId, int days)
        {
            var balances = await _accountRepository.GetBalances(userId, accountId, days);

            List<BalanceResponse> response = balances.Select(balance => _mapper.Map<BalanceResponse>(balance)).ToList();

            return response;
        }

        public async Task<IEnumerable<TransactionResponse>> GetTransactions(int userId, int accountId, int days)
        {
            var transactions = await _accountRepository.GetTransactions(userId, accountId, days);

            List<TransactionResponse> response = transactions.Select(acc => _mapper.Map<TransactionResponse>(acc)).ToList();

            return response;
        }

        public async Task<TransactionResponse> ExecuteTransaction(TransactionType type, int userId, int accountId, decimal Amount, string barCode = null)
        {
            var account = await _accountRepository.GetAccount(userId, accountId);

            bool canExecuteOperation = false;

            switch (type)
            {
                case TransactionType.Payment:
                    canExecuteOperation = account.CanExecutePayment(Amount, barCode);
                    break;
                case TransactionType.Deposit:
                    canExecuteOperation = account.CanExecuteDeposit(Amount);
                    break;
                case TransactionType.Withdraw:
                    canExecuteOperation = account.CanExecuteWithdraw(Amount);
                    break;
            }

            if (!canExecuteOperation)
            {
                return new TransactionResponse() { Errors = account.Errors };
            }

            var transaction = await _transactionRepository.ExecuteTransaction(type, accountId, Amount, account.Balance);

            await _accountRepository.UpdateBalance(userId, account.AccountId, account.Balance);

            return _mapper.Map<TransactionResponse>(transaction);
        }

        public async Task ExecuteBalanceUpdate()
        {
            await _accountRepository.ExecuteBalanceUpdate();
        }
    }
}
