using NakedBank.Domain;
using System.Threading.Tasks;

namespace NakedBank.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUser(string username);
        Task<User> SaveUser(User user);
        Task<User> UpdateUser(User user);
        Task<bool> RemoveUser(string username);

        Task UpdateUserLastAccess(string username);
        Task SaveToken(Token token);
    }
}
