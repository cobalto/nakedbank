using NakedBank.Domain;
using NakedBank.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NakedBank.Application.Tests
{
    public class AccountServiceTests : IClassFixture<ServiceFixture>
    {
        ServiceFixture _fixture;
        public const string BARCODE_OK = "12345678901234567890123456789012345678901234";
        public const string BARCODE_FAIL = "1234567890123456789012345678901234567890123";

        public AccountServiceTests(ServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void GetAccountsMustSucceed()
        {
            User user = _fixture.DefaultUser;

            var results = await _fixture.AccountService.GetAccounts(user.UserId);

            Assert.NotEmpty(results);
        }

        [Fact]
        public async void GetBalancesMustSucceed()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.GetBalances(user.UserId, account.AccountId, 5);

            Assert.NotEmpty(results);
        }

        [Fact]
        public async void GetTransactionsMustSucceed()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.GetTransactions(user.UserId, account.AccountId, 5);

            Assert.NotEmpty(results);
        }

        [Fact]
        public async void TransactionPaymentMustSucceed()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Payment,
                user.UserId, account.AccountId, 200, BARCODE_OK);

            Assert.Empty(results.Errors);
        }

        [Fact]
        public async void TransactionDepositMustSucceed()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Deposit,
                user.UserId, account.AccountId, 200);

            Assert.Empty(results.Errors);
        }

        [Fact]
        public async void TransactionWithdrawMustSucceed()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Withdraw,
                user.UserId, account.AccountId, 200);

            Assert.Empty(results.Errors);
        }

        [Fact]
        public async void TransactionPaymentBarcodeMustFail()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Payment,
                user.UserId, account.AccountId, 200, BARCODE_FAIL);

            Assert.NotEmpty(results.Errors);
            Assert.Contains("barcode", results.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public async void TransactionPaymentFoundsMustFail()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Payment,
                user.UserId, account.AccountId, 1200, BARCODE_OK);

            Assert.NotEmpty(results.Errors);
            Assert.Contains("founds", results.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public async void TransactionWithdrawFoundsMustFail()
        {
            User user = _fixture.DefaultUser;
            Account account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Withdraw,
                user.UserId, account.AccountId, 1200);

            Assert.NotEmpty(results.Errors);
            Assert.Contains("founds", results.Errors.FirstOrDefault().Message);
        }
    }
}
