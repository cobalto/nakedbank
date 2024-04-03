using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NakedBank.Infrastructure.Entities;
using NakedBank.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NakedBank.Infrastructure
{
    public static class Bootstrapper
    {
        public static IHost DatabaseStartup(this IHost host)
        {
            InitializeDatabase(host);
            return host;
        }

        [Conditional("DEBUG")]
        private static void InitializeDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<NakedContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                SeedUsers(context);
                SeedAccounts(context);
                SeedBalances(context);
                SeedTransactions(context);
            }
        }

        private static void SeedUsers(NakedContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Login = "12345678900",
                        Password = "02C642C83E3979E94D76F39A01BEB91E5AAB8D135DD4F6A7FCF35BCF75545F0F",
                        FirstName = "Mary",
                        LastName = "Doe",
                        EmailAddress = "mary@doe.com",
                        PhoneNumber = "+1 12 987-654-321",
                        Active = true,
                        LastAccessAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Login = "12345678901",
                        Password = "02C642C83E3979E94D76F39A01BEB91E5AAB8D135DD4F6A7FCF35BCF75545F0F",
                        FirstName = "John",
                        LastName = "Smith",
                        EmailAddress = "john@smith.com",
                        PhoneNumber = "+1 12 123-456-789",
                        Active = true,
                        LastAccessAt = DateTime.UtcNow.AddMonths(-15)
                    }
                };

                context.AddRange(users);
                context.SaveChanges();
            }
        }

        private static void SeedAccounts(NakedContext context)
        {
            if (!context.Accounts.Any())
            {
                var accounts = new List<Account>();

                string defaultBranch = "1";

                foreach (var user in context.Users.ToList())
                {
                    for (int i = 0; i < new Random().Next(2, 4); i++)
                    {
                        var acc = new Account()
                        {
                            UserId = user.UserId,
                            AccountNumber = $"{defaultBranch.PadRight(3, '0')}9{user.UserId.ToString().PadRight(5, '0')}{i}",
                            BranchId = 1,
                            Balance = 85,
                        };

                        accounts.Add(acc);
                    }
                }

                context.AddRange(accounts);
                context.SaveChanges();
            }
        }

        private static void SeedBalances(NakedContext context)
        {
            if (!context.Balances.Any())
            {
                foreach (var account in context.Accounts.ToList())
                {
                    var balances = new List<Balance>
                    {
                        new Balance { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-1), Amount = 60 },
                        new Balance { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-2), Amount = 85 },
                        new Balance { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-3), Amount = 50 },
                        new Balance { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-4), Amount = 280 },
                        new Balance { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-5), Amount = 580 },
                        new Balance { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-6), Amount = 780 },
                        new Balance { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-7), Amount = 800 },
                    };

                    context.AddRange(balances);
                }
                context.SaveChanges();
            }
        }

        private static void SeedTransactions(NakedContext context)
        {
            if (!context.Transactions.Any())
            {
                foreach (var account in context.Accounts.ToList())
                {
                    var balances = new List<Transaction>
                    {
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-0), Amount = 100, BalanceAfterTransaction = 85, TransactionType = (int)TransactionType.Deposit },
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-0), Amount = 75, BalanceAfterTransaction = -15, TransactionType = (int)TransactionType.Payment },
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-2), Amount = 25, BalanceAfterTransaction = 60, TransactionType = (int)TransactionType.Withdraw },
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-3), Amount = 35, BalanceAfterTransaction = 85, TransactionType = (int)TransactionType.Deposit },
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-4), Amount = 230, BalanceAfterTransaction = 50, TransactionType = (int)TransactionType.Payment },
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-5), Amount = 300, BalanceAfterTransaction = 280, TransactionType = (int)TransactionType.Withdraw },
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-6), Amount = 200, BalanceAfterTransaction = 580, TransactionType = (int)TransactionType.Withdraw },
                        new Transaction { AccountId = account.AccountId, Timestamp = DateTime.Now.Date.AddDays(-7), Amount = 20, BalanceAfterTransaction = 780, TransactionType = (int)TransactionType.Withdraw },
                    };

                    context.AddRange(balances);
                }
                context.SaveChanges();
            }
        }
    }
}
