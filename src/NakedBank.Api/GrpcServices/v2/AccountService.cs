using Grpc.Core;
using NakedBank.Api.v2;
using NakedBank.Application.Interfaces;

namespace NakedBank.Api.GrpcServices.v2
{
    public class AccountService : AccountServicer.AccountServicerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ILogger<GreeterService> _logger;

        public AccountService(ILogger<GreeterService> logger,
            IUserService userService,
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<BalanceResponse> GetBalances(GetBalanceRequest request, ServerCallContext context)
        {
            return base.GetBalances(request, context);
        }

        public override Task<TransactionResponse> GetRecentTransactions(GetTransactionRequest request, ServerCallContext context)
        {
            return base.GetRecentTransactions(request, context);
        }

        public override Task<TransactionResponse> PostTransactionAsync(PostTransactionRequest request, ServerCallContext context)
        {
            return base.PostTransactionAsync(request, context);
        }
    }
}
