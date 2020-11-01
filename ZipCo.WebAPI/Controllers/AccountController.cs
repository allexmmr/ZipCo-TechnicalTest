using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ZipCo.WebAPI.Services.Interface;
using ZipCo.WebAPI.ViewModels;

namespace ZipCo.WebAPI.Controllers
{
    /// <summary>
    /// Account Controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IService<AccountResponse, AccountRequest> _accountService;

        /// <summary>
        /// Account Controller Constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="accountService">Account Service</param>
        public AccountController(ILogger<AccountController> logger, IService<AccountResponse, AccountRequest> accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        // GET
        // api/account/list
        /// <summary>
        /// List all accounts.
        /// </summary>
        /// <returns>Return a list of accounts.</returns>
        [HttpGet("list")]
        public IActionResult List()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(List));

            try
            {
                return Ok(_accountService.List());
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(List), ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST
        // api/account/create
        /// <summary>
        /// Create an account.
        /// </summary>
        /// <param name="request">Account View Model</param>
        /// <returns>Return the created account.</returns>
        [HttpPost("create")]
        public IActionResult Create([FromBody] AccountRequest request)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Create));

            try
            {
                AccountResponse account = _accountService.Create(request);

                return account.Success
                    ? (IActionResult)Ok(account)
                    : BadRequest(account.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(Create), ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
