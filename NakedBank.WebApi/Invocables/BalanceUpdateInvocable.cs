using Coravel.Invocable;
using NakedBank.Application.Interfaces;
using System.Threading.Tasks;

namespace NakedBank.WebApi.Invocables
{
    public class BalanceUpdateInvocable : IInvocable
    {
        private readonly IAccountService _accountService;

        public BalanceUpdateInvocable(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Invoke()
        {
            await _accountService.ExecuteBalanceUpdate();
        }
    }
}
