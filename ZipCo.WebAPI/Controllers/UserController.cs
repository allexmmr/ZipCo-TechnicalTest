using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ZipCo.WebAPI.Services.Interface;
using ZipCo.WebAPI.ViewModels;

namespace ZipCo.WebAPI.Controllers
{
    /// <summary>
    /// User Controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        /// <summary>
        /// User Controller Constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="userService">User Service</param>
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        // GET
        // api/user/list
        /// <summary>
        /// List all users.
        /// </summary>
        /// <returns>Return a list of users.</returns>
        [HttpGet("list")]
        public IActionResult List()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(List));

            try
            {
                return Ok(_userService.List());
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(List), ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST
        // api/user/create
        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="request">User request</param>
        /// <returns>Return the created user.</returns>
        [HttpPost("create")]
        public IActionResult Create([FromBody] UserRequest request)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Create));

            try
            {
                UserResponse user = _userService.Create(request);

                return user.Success
                    ? (IActionResult)Ok(user)
                    : BadRequest(user.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(Create), ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET
        // api/user/get/{emailAddress}
        /// <summary>
        /// Get a user by email address.
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Return a user.</returns>
        [HttpGet("{emailAddress}")]
        public IActionResult Get(string emailAddress)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Get));

            try
            {
                UserResponse user = _userService.Get(emailAddress);

                return user.Success
                    ? (IActionResult)Ok(user)
                    : BadRequest(user.ErrorMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(Get), ex);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}