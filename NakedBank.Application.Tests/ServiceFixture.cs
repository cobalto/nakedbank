using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NakedBank.Application.Interfaces;
using NakedBank.Application.Repositories;
using NakedBank.Application.Services;
using NakedBank.Domain;
using NakedBank.Shared.Models;
using NakedBank.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();

                mockUserRepository.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(DefaultUser);
                mockUserRepository.Setup(x => x.UpdateUserLastAccess(It.IsAny<string>())).Verifiable();
                mockUserRepository.Setup(x => x.SaveToken(It.IsAny<Token>())).Verifiable();

                return mockUserRepository.Object;
            });

            service.AddSingleton<IAccountRepository>(_ =>
            {
                Mock<IAccountRepository> mockAccountRepository = new Mock<IAccountRepository>();

                mockAccountRepository.Setup(x => x.GetAccount(It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync((int user, int acc) => new Account(acc, 1, "001", 500, 1));

                mockAccountRepository.Setup(x => x.GetAccounts(It.IsAny<int>()))
                    .ReturnsAsync(DefaultAccounts);

                mockAccountRepository.Setup(x => x.GetBalances(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(DefaultBalances);

                mockAccountRepository.Setup(x => x.GetTransactions(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync(DefaultTransactions);


                mockAccountRepository.Setup(x => x.ExecuteBalanceUpdate()).Verifiable();
                mockAccountRepository.Setup(x => x.UpdateBalance(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>())).Verifiable();
                mockAccountRepository.Setup(x => x.ExecuteBalanceUpdate()).Verifiable();

                return mockAccountRepository.Object;
            });

            service.AddSingleton<ITransactionRepository>(_ =>
            {
                Mock<ITransactionRepository> mockTransactionRepository = new Mock<ITransactionRepository>();

                mockTransactionRepository.Setup(x => x.ExecuteTransaction(
                    It.IsAny<TransactionType>(), It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<string>()))
                        .ReturnsAsync((TransactionType type, int acc, decimal amount, decimal balance, string barcode) =>
                            new Transaction(Guid.NewGuid(), type, amount, DateTime.UtcNow, acc));

                return mockTransactionRepository.Object;
            });

            service.AddSingleton<IConfigurationRepository>(_ =>
            {
                Mock<IConfigurationRepository> mockConfigurationRepository = new Mock<IConfigurationRepository>();

                mockConfigurationRepository.Setup(x => x.GetConfig(It.IsAny<string>())).Returns(DEFAULT_SECRET);

                return mockConfigurationRepository.Object;
            });

            var provider = service.BuildServiceProvider();

            UserService = provider.GetService<IUserService>();
            AccountService = provider.GetService<IAccountService>();
        }
    }
}
