using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using NakedBank.Application.Repositories;
using NakedBank.Application.Interfaces;
using NakedBank.Application.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NakedBank.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace NakedBank.Application.Tests
{
    public class UserServiceTests : IClassFixture<ServiceFixture>
    {
        ServiceFixture _fixture;

        public UserServiceTests(ServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AuthenticationMustSucceed()
        {
            User user = _fixture.DefaultUser;

            var authResult = await _fixture.UserService.Authenticate(user.Login.ToString(), "naked1234naked");

            Assert.Equal(user.Login.ToString(), authResult.Login);
            Assert.Equal(user.UserId, authResult.UserId);
            Assert.NotNull(authResult.Token);
            Assert.Empty(authResult.Errors);
        }

        [Fact]
        public async Task AuthenticationMustFail()
        {
            User user = _fixture.DefaultUser;

            var authResult = await _fixture.UserService.Authenticate(user.Login.ToString(), "naked12345naked");

            Assert.Null(authResult.Token);
            Assert.NotEmpty(authResult.Errors);
            Assert.Contains("not found", authResult.Errors.FirstOrDefault().Message);
        }

        [Fact]
        public async Task GetUserProfileMustSucceed()
        {
            User user = _fixture.DefaultUser;
            
            var userResult = await _fixture.UserService.GetUserProfile(user.Login.ToString());

            Assert.Equal(user.Login.ToString(), userResult.Login);
            Assert.Equal(user.UserId, userResult.UserId);
            Assert.Equal(user.EmailAddress.ToString(), userResult.EmailAddress);
            Assert.Empty(userResult.Errors);
        }

        [Fact]
        public async Task GetUserIdMustSucceed()
        {
            User user = _fixture.DefaultUser;

            var resultId = await _fixture.UserService.GetUserId(user.Login.ToString());

            Assert.Equal(user.UserId, resultId);
        }
        
    }
}
