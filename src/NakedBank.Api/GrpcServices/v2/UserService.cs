using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NakedBank.Api.v2;
using NakedBank.Application.Interfaces;
using System.Security.Claims;

namespace NakedBank.Api.GrpcServices.v2
{
    public class UserService : UserServicer.UserServicerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ILogger<GreeterService> _logger;

        public UserService(ILogger<GreeterService> logger,
            IUserService userService,
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<ProfileResponse> GetCurrentUser(Empty request, ServerCallContext context)
        {
            try
            {
                var username = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var profile = await _userService.GetUserProfile(username);

                // Continue with your gRPC service logic...

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public override async Task<AccountResponse> GetUserAccounts(Empty request, ServerCallContext context)
        {
            try
            {
                var username = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var userId = await _userService.GetUserId(username);

                var accounts = await _accountService.GetAccounts(userId);

                // Continue with your gRPC service logic...

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public override async Task<AuthResponse> Authenticate(AuthRequest request, ServerCallContext context)
        {
            try
            {
                var auth = await _userService.Authenticate(request.Login, request.Password);

                if (auth is null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Username or password invalid"));
                }

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
