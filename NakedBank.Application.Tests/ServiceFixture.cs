using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NakedBank.Application.Interfaces;
using NakedBank.Application.Repositories;
using NakedBank.Application.Services;
using NakedBank.Domain;
using NakedBank.Shared.Models;
using NSubstitute;

namespace NakedBank.Application.Tests
{
    public class ServiceFixture
    {
        public IUserService UserService;
        public IAccountService AccountService;

        public User DefaultUser = new User(
                    1, "John", "Smith", new Login("12312312300"), new Password("naked1234naked"),
                    new Email("john@smith.com"), new PhoneNumber("+5551123456789"), DateTime.UtcNow);

        public List<Account> DefaultAccounts = new List<Account>()
        {
            new Account(1, 1, "001", 100, 1),
            new Account(1, 2, "002", 200, 1)
        };

        public IEnumerable<Balance> DefaultBalances = new List<Balance>()
        {
            new Balance(1, 100, DateTime.UtcNow.AddDays(-3), 1),
            new Balance(1, 75, DateTime.UtcNow.AddDays(-2), 1),
            new Balance(1, 25, DateTime.UtcNow.AddDays(-1), 1),
        };

        public IEnumerable<Transaction> DefaultTransactions = new List<Transaction>()
        {
            new Transaction(Guid.NewGuid(), TransactionType.Deposit, 100, DateTime.UtcNow, 1),
            new Transaction(Guid.NewGuid(), TransactionType.Payment, 50, DateTime.UtcNow.AddDays(-1), 1),
            new Transaction(Guid.NewGuid(), TransactionType.Withdraw, 25, DateTime.UtcNow.AddDays(-2), 1),
        };

        private const string DEFAULT_SECRET = "xEzq98jGYb@DpaNsH9G?uT4KtsY-7B2P";

        public ServiceFixture()
        {
            var service = new ServiceCollection();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });

            IMapper mapper = config.CreateMapper();
            service.AddSingleton(mapper);

            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IAccountService, AccountService>();

            service.AddSingleton<IUserRepository>(_ =>
            {
                var mockUserRepository = Substitute.For<IUserRepository>();

                mockUserRepository.GetUser(Arg.Any<string>()).Returns(Task.FromResult(DefaultUser));

                return mockUserRepository;
            });

            service.AddSingleton<IAccountRepository>(_ =>
            {
                //Mock<IAccountRepository> mockAccountRepository = new Mock<IAccountRepository>();
                var mockAccountRepository = Substitute.For<IAccountRepository>();

                mockAccountRepository.GetAccount(Arg.Any<int>(), Arg.Any<int>())
                    .Returns(callInfo =>
                    {
                        int user = callInfo.ArgAt<int>(0);
                        int acc = callInfo.ArgAt<int>(1);

                        return Task.FromResult(new Account(acc, 1, "001", 500, 1));
                    });

                mockAccountRepository.GetAccounts(Arg.Any<int>())
                    .Returns((IEnumerable<Account>)Task.FromResult(DefaultAccounts));

                mockAccountRepository.GetBalances(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>())
                    .Returns(Task.FromResult(DefaultBalances));

                mockAccountRepository.GetTransactions(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>())
                    .Returns(Task.FromResult(DefaultTransactions));

                return mockAccountRepository;
            });

            service.AddSingleton<ITransactionRepository>(_ =>
            {
                var mockTransactionRepository = Substitute.For<ITransactionRepository>();

                mockTransactionRepository.ExecuteTransaction(Arg.Any<TransactionType>(), Arg.Any<int>(), Arg.Any<decimal>(), Arg.Any<decimal>(), Arg.Any<string>())
                    .Returns(callInfo =>
                    {
                        TransactionType type = callInfo.ArgAt<TransactionType>(0);
                        int acc = callInfo.ArgAt<int>(1);
                        decimal amount = callInfo.ArgAt<decimal>(2);
                        decimal balance = callInfo.ArgAt<decimal>(3);
                        string barcode = callInfo.ArgAt<string>(4);

                        return Task.FromResult(new Transaction(Guid.NewGuid(), type, amount, DateTime.UtcNow, acc));
                    });

                return mockTransactionRepository;
            });

            service.AddSingleton<IConfigurationRepository>(_ =>
            {
                var mockConfigurationRepository = Substitute.For<IConfigurationRepository>();

                mockConfigurationRepository.GetConfig(Arg.Any<string>()).Returns(DEFAULT_SECRET);

                return mockConfigurationRepository;
            });

            var provider = service.BuildServiceProvider();

            UserService = provider.GetService<IUserService>();
            AccountService = provider.GetService<IAccountService>();
        }
    }
}
