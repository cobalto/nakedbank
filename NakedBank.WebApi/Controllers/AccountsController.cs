using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NakedBank.Application.Interfaces;
using NakedBank.Shared.Models.Requests;
using NakedBank.Shared.Models.Responses;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NakedBank.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public AccountsController(ILogger<UsersController> logger,
            IUserService userService,
            IAccountService accountService)
        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
        }

        [HttpGet, Route("{accountId}/balances")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBalances(int accountId, int days = 7)
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var userId = await _userService.GetUserId(username);

                var balances = await this._accountService.GetBalances(userId, accountId, days);

                if (balances.SelectMany(a => a.Errors).Any())
                {
                    return BadRequest(balances);
                }

                return Ok(balances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        [HttpGet, Route("{accountId}/transactions")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRecentTransactions(int accountId, int days = 3)
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var userId = await _userService.GetUserId(username);

                var transactions = await this._accountService.GetTransactions(userId, accountId, days);

                if (transactions.SelectMany(a => a.Errors).Any())
                {
                    return BadRequest(transactions);
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        [HttpPost, Route("{accountId}/transactions")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTransactionAsync(int accountId, [FromBody] TransactionRequest request)
        {
            try
            {
                var username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

                var userId = await _userService.GetUserId(username);

                TransactionResponse response = await this._accountService.ExecuteTransaction(request.TransactionType, userId, accountId, request.Amount, request.Barcode);

                if (response.Errors.Any())
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return StatusCode(500);
            }
        }
    }
}
