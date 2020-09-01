using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using NakedBank.Application.Interfaces;
using NakedBank.Application.Repositories;
using NakedBank.Domain;
using NakedBank.Shared.Models.Responses;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NakedBank.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IConfigurationRepository _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IConfigurationRepository configuration,
            IUserRepository userReposistory,
            IMapper mapper)
        {
            _configuration = configuration;
            _userRepository = userReposistory;
            _mapper = mapper;
        }

        public async Task<AuthResponse> Authenticate(string username, string password)
        {
            var response = new AuthResponse();

            var user = await _userRepository.GetUser(username);

            if (user == null || !user.Password.IsEquals(new Domain.Password(password)))
            {
                response.Errors.Add(new Shared.BusinessError("Username/Password", "User not found for credentials"));
                return response;
            }
            else
            {
                response.UserId = user.UserId;
                response.Login = user.Login.ToString();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration.GetConfig("AuthSettings:Secret"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Login.ToString()),
                    new Claim(ClaimTypes.Email, user.EmailAddress.ToString())
                    }),
                    IssuedAt = DateTime.UtcNow,
                    NotBefore = DateTime.UtcNow.AddSeconds(1),
                    Expires = DateTime.UtcNow.AddHours(3),

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Token = tokenHandler.WriteToken(token);

                await this.UpdateUserSecurity(user, tokenDescriptor, response.Token);

                await _userRepository.UpdateUserLastAccess(user.Login.ToString());

                return response;
            }
        }

        private async Task UpdateUserSecurity(User user, SecurityTokenDescriptor tokenDescriptor, string token)
        {
            await _userRepository.SaveToken(new Token(
                    user.UserId,
                    token,
                    (DateTime)tokenDescriptor.IssuedAt,
                    (DateTime)tokenDescriptor.NotBefore,
                    (DateTime)tokenDescriptor.Expires
                ));
        }

        public async Task<ProfileResponse> GetUserProfile(string username)
        {
            var user = await _userRepository.GetUser(username);

            ProfileResponse response = _mapper.Map<ProfileResponse>(user);

            return response;
        }

        public async Task<int> GetUserId(string username)
        {
            var user = await _userRepository.GetUser(username);

            return user.UserId;
        }
    }
}
