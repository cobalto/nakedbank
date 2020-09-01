using NakedBank.Front.Models;
using NakedBank.Shared.Models.Requests;
using NakedBank.Shared.Models.Responses;
using System.Threading.Tasks;

namespace NakedBank.Front.Services
{
    public interface IAuthenticationService
    {
        Task<User> Login(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private IHttpService _httpService;

        public AuthenticationService(
            IHttpService httpService
        ) {
            _httpService = httpService;
        }

        public async Task<User> Login(string username, string password)
        {
            User user = new User();

            user.Authorization = await _httpService.Post<AuthResponse>("users/authenticate", new AuthRequest() { Login = "12345678900", Password = "naked1234naked" });
            user.Profile = await _httpService.Get<ProfileResponse>("users/profile", user.Authorization.Token);

            return user;
        }
    }
}