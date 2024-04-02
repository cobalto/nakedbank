using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NakedBank.Application.Repositories;
using NakedBank.Infrastructure.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NakedBank.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NakedContext _context;
        private readonly IMapper _mapper;

        public UserRepository(NakedContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Domain.User> GetUser(string username)
        {
            var user = await _context.Users
                                     .Where(x => x.Active)
                                     .SingleOrDefaultAsync(x => x.Login == username);

            if (user == null)
            {
                return null;
            }

            //var result = _mapper.Map<Domain.User>(user);
            var result = new Domain.User(user.UserId,
                user.FirstName,
                user.LastName,
                new Domain.Login(user.Login),
                new Domain.Password(new Domain.Hash(user.Password)),
                new Domain.Email(user.EmailAddress),
                new Domain.PhoneNumber(user.PhoneNumber),
                user.LastAccessAt);

            return result;
        }

        public async Task<Domain.User> SaveUser(Domain.User user)
        {
            var newUser = _mapper.Map<User>(user);
            newUser.Active = true;
            newUser.LastAccessAt = DateTime.UtcNow;

            var addedUser = await _context.Users.AddAsync(newUser);

            await _context.SaveChangesAsync();

            var result = _mapper.Map<Domain.User>(addedUser);

            return result;
        }

        public async Task<Domain.User> UpdateUser(Domain.User user)
        {
            var currentUser = await _context.Users.SingleOrDefaultAsync(x => x.Login == user.Login.ToString());

            if (currentUser == null)
            {
                return null;
            }

            currentUser.PhoneNumber = user.PhoneNumber.ToString();
            currentUser.EmailAddress = user.EmailAddress.ToString();

            await _context.SaveChangesAsync();

            //var result = _mapper.Map<Domain.User>(currentUser);
            var result = new Domain.User(currentUser.UserId,
                currentUser.FirstName,
                currentUser.LastName,
                new Domain.Login(currentUser.Login),
                new Domain.Password(new Domain.Hash(currentUser.Password)),
                new Domain.Email(currentUser.EmailAddress),
                new Domain.PhoneNumber(currentUser.PhoneNumber),
                user.LastAccessAt);

            return result;
        }

        public async Task<bool> RemoveUser(string username)
        {
            var user = await _context.Users
                                     .Where(x => x.Active)
                                     .SingleOrDefaultAsync(x => x.Login == username);

            if (user == null)
            {
                return false;
            }

            user.Active = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdateUserLastAccess(string username)
        {
            var user = await _context.Users
                                     .Where(x => x.Active)
                                     .SingleOrDefaultAsync(x => x.Login == username);

            user.LastAccessAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SaveToken(Domain.Token token)
        {
            Token newToken = _mapper.Map<Token>(token);

            await _context.Tokens.AddAsync(newToken);

            await _context.SaveChangesAsync();
        }
    }
}
