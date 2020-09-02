using NakedBank.Front.Models;
using NakedBank.Shared.Models.Requests;
using NakedBank.Shared.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NakedBank.Front.Services
{
    public interface INakedService
    {
        Task<IEnumerable<AccountResponse>> GetAccounts();
        Task<IEnumerable<BalanceResponse>> GetBalances(int accountId, int days = 7);
        Task<IEnumerable<TransactionResponse>> GetTransactions(int accountId, int days = 3);
        Task<TransactionResponse> SendTransaction(int accountId, TransactionRequest request);
    }

    public class NakedService : INakedService
    {
        private IHttpService _httpService;
        private ILocalStorageService _localStorageService;

        public NakedService(IHttpService httpService,
            ILocalStorageService localStorageService)
        {
            _httpService = httpService;
            _localStorageService = localStorageService;
        }

        public async Task<IEnumerable<AccountResponse>> GetAccounts()
        {
            var token = (await _localStorageService.GetItem<User>("User")).Authorization.Token;
            return await _httpService.Get<IEnumerable<AccountResponse>>("users/accounts", token);
        }

        public async Task<IEnumerable<BalanceResponse>> GetBalances(int accountId, int days = 7)
        {
            var token = (await _localStorageService.GetItem<User>("User")).Authorization.Token;
            return await _httpService.Get<IEnumerable<BalanceResponse>>($"accounts/{accountId}/balances?days={days}", token);
        }

        public async Task<IEnumerable<TransactionResponse>> GetTransactions(int accountId, int days = 3)
        {
            var token = (await _localStorageService.GetItem<User>("User")).Authorization.Token;
            return await _httpService.Get<IEnumerable<TransactionResponse>>($"accounts/{accountId}/transactions?days={days}", token);
        }

        public async Task<TransactionResponse> SendTransaction(int accountId, TransactionRequest request)
        {
            var token = (await _localStorageService.GetItem<User>("User")).Authorization.Token;
            return await _httpService.Post<TransactionResponse>($"accounts/{accountId}/transactions", request, token);
        }
    }
}