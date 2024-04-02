using NakedBank.Shared.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NakedBank.Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> Authenticate(string username, string password);
        Task<ProfileResponse> GetUserProfile(string username);
        Task<int> GetUserId(string username);
    }
}
