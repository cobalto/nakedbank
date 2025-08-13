using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NakedBank.Application.Tests
{
    public class AccountServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceFixture _fixture;
        private const string BarcodeOk = "12345678901234567890123456789012345678901234";
        private const string BarcodeFail = "1234567890123456789012345678901234567890123";

        public AccountServiceTests(ServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAccountsMustSucceed()
        {
            var user = _fixture.DefaultUser;

            var results = await _fixture.AccountService.GetAccounts(user.UserId);

            Assert.NotEmpty(results);
        }

        [Fact]
        public async Task GetBalancesMustSucceed()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.GetBalances(user.UserId, account.AccountId, 5);

            Assert.NotEmpty(results);
        }

        [Fact]
        public async Task GetTransactionsMustSucceed()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.GetTransactions(user.UserId, account.AccountId, 5);

            Assert.NotEmpty(results);
        }

        [Fact]
        public async Task TransactionPaymentMustSucceed()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Payment,
                user.UserId, account.AccountId, 200, BarcodeOk);

            Assert.Empty(results.Errors);
        }

        [Fact]
        public async Task TransactionDepositMustSucceed()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Deposit,
                user.UserId, account.AccountId, 200);

            Assert.Empty(results.Errors);
        }

        [Fact]
        public async Task TransactionWithdrawMustSucceed()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Withdraw,
                user.UserId, account.AccountId, 200);

            Assert.Empty(results.Errors);
        }

        [Fact]
        public async Task TransactionPaymentBarcodeMustFail()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Payment,
                user.UserId, account.AccountId, 200, BarcodeFail);

            Assert.NotEmpty(results.Errors);
            Assert.Contains("barcode", results.Errors.FirstOrDefault()?.Message);
        }

        [Fact]
        public async Task TransactionPaymentFoundsMustFail()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Payment,
                user.UserId, account.AccountId, 1200, BarcodeOk);

            Assert.NotEmpty(results.Errors);
            Assert.Contains("founds", results.Errors.FirstOrDefault()?.Message);
        }

        [Fact]
        public async Task TransactionWithdrawFoundsMustFail()
        {
            var user = _fixture.DefaultUser;
            var account = _fixture.DefaultAccounts.FirstOrDefault();

            var results = await _fixture.AccountService.ExecuteTransaction(Shared.Models.TransactionType.Withdraw,
                user.UserId, account.AccountId, 1200);

            Assert.NotEmpty(results.Errors);
            Assert.Contains("founds", results.Errors.FirstOrDefault()?.Message);
        }
    }
}
