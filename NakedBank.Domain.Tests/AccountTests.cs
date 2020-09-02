using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NakedBank.Domain.Tests
{
    public class AccountTests
    {
        public const int ACC_ID = 1;
        public const int BRANCH_ID = 1;
        public const string ACC_NUM = "00027";
        public const decimal INI_BALANCE = 500;
        public const int USER_ID = 1;
        
        public const string BAR_CODE_OK = "12345678901234567890123456789012345678901234";
        public const string BAR_CODE_FAIL = "1234567890123456789012345678901234567890123";


        public Account GetAccount()
        {
            return new Account(ACC_ID, BRANCH_ID, ACC_NUM, INI_BALANCE, USER_ID);
        }

        [Fact]
        public void DepositkMustSucceed()
        {
            var account = GetAccount();

            var result = account.CanExecuteDeposit(150);

            Assert.True(result);
            Assert.Equal(INI_BALANCE + 150, account.Balance);
            Assert.False(account.HasErrors);
        }

        [Fact]
        public void PaymentWithValidBarcodeMustSucceed()
        {
            var account = GetAccount();

            var result = account.CanExecutePayment(200, BAR_CODE_OK);

            Assert.True(result);
            Assert.Equal(INI_BALANCE - 200, account.Balance);
            Assert.False(account.HasErrors);
        }

        [Fact]
        public void WithdrawMustSucceed()
        {
            var account = GetAccount();

            var result = account.CanExecuteWithdraw(400);

            Assert.True(result);
            Assert.Equal(INI_BALANCE - 400, account.Balance);
            Assert.False(account.HasErrors);
        }

        [Fact]
        public void PaymentWithInsuficientFoundsMustFail()
        {
            var account = GetAccount();

            var result = account.CanExecutePayment(555, BAR_CODE_OK);

            Assert.False(result);
            Assert.Equal(INI_BALANCE, account.Balance);
            Assert.True(account.HasErrors);
            Assert.Contains("founds", account.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public void PaymentWithInvalidBarcodeMustFail()
        {
            var account = GetAccount();

            var result = account.CanExecutePayment(200, BAR_CODE_FAIL);

            Assert.False(result);
            Assert.Equal(INI_BALANCE, account.Balance);
            Assert.True(account.HasErrors);
            Assert.Contains("barcode", account.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public void WithdrawWithInsuficientFoundsMustFail()
        {
            var account = GetAccount();

            var result = account.CanExecuteWithdraw(800);

            Assert.False(result);
            Assert.Equal(INI_BALANCE, account.Balance);
            Assert.True(account.HasErrors);
            Assert.Contains("founds", account.Errors.FirstOrDefault().Message);
        }
    }
}
