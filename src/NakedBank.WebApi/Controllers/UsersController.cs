using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NakedBank.Application.Interfaces;
using NakedBank.Shared.Models.Requests;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NakedBank.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UsersController(ILogger<UsersController> logger,
            IUserService userService,
            IAccountService accountService)
        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
        }

        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var profile = await _userService.GetUserProfile(username);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        [HttpGet("accounts")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserAccounts()
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var userId = await _userService.GetUserId(username);

                var accounts = await _accountService.GetAccounts(userId);

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            try
            {
                var auth = await _userService.Authenticate(request.Login, request.Password);

                if (auth is null)
                {
                    return BadRequest(new { message = "Username or password invalid" });
                }

                return Ok(auth);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return StatusCode(500);
            }
        }
    }
}
