using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using ZipCo.Common.Domain;
using ZipCo.Data.Entities;
using ZipCo.Data.Repositories.Interfaces;
using ZipCo.WebAPI.Services.Interface;
using ZipCo.WebAPI.ViewModels;

namespace ZipCo.WebAPI.Services
{
    /// <summary>
    /// User Service Class.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// User Service Constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="userRepository">User Repository</param>
        public UserService(ILogger<UserService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// List all users.
        /// </summary>
        /// <returns>Return a list of users.</returns>
        public IEnumerable<UserResponse> List()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(List));

            try
            {
                IEnumerable<User> users = _userRepository.List();

                return users.Select(x => new UserResponse
                {
                    User = new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        EmailAddress = x.EmailAddress,
                        MonthlySalary = x.MonthlySalary,
                        MonthlyExpenses = x.MonthlyExpenses
                    },
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(List), ex);

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="request">User request</param>
        /// <returns>Return the created user.</returns>
        public UserResponse Create(UserRequest request)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Create));

            try
            {
                UserResponse response = null;

                if (!IsModelValid(request, ref response)
                    || !IsUniqueEmailAddress(request.EmailAddress, ref response))
                {
                    return response;
                }

                User user = _userRepository.Create(new User
                {
                    Name = request.Name,
                    EmailAddress = request.EmailAddress,
                    Password = Encryptor.EncryptMD5(request.Password),
                    MonthlySalary = request.MonthlySalary,
                    MonthlyExpenses = request.MonthlyExpenses
                });

                response.User = user;
                response.User.Id = user.Id;
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(Create), ex);

                return new UserResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Get a user by email address.
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Return a user.</returns>
        public UserResponse Get(string emailAddress)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Get));

            try
            {
                UserResponse model = null;

                if (!IsEmailAddressProvided(emailAddress, ref model))
                {
                    return model;
                }

                User user = _userRepository.Get(emailAddress);

                if (!IsUserFound(user, ref model))
                {
                    return model;
                }

                return new UserResponse
                {
                    User = new User
                    {
                        EmailAddress = user.EmailAddress,
                        Id = user.Id,
                        MonthlyExpenses = user.MonthlyExpenses,
                        MonthlySalary = user.MonthlySalary,
                        Name = user.Name
                    },
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(Get), ex);

                throw;
            }
        }

        #region Validations

        private bool IsModelValid(UserRequest request, ref UserResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsModelValid));

            if (request == null)
            {
                response = new UserResponse
                {
                    ErrorMessage = "User data not provided.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        private bool IsUniqueEmailAddress(string emailAddress, ref UserResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsUniqueEmailAddress));

            User user = _userRepository.List().FirstOrDefault(x => string.Equals(x.EmailAddress, emailAddress, StringComparison.CurrentCultureIgnoreCase));

            if (user != null)
            {
                response = new UserResponse
                {
                    ErrorMessage = "User already exists for this email.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        private bool IsEmailAddressProvided(string emailAddress, ref UserResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsEmailAddressProvided));

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                response = new UserResponse
                {
                    ErrorMessage = "Email address not provided.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        private bool IsUserFound(User user, ref UserResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsUserFound));

            if (user == null)
            {
                response = new UserResponse
                {
                    ErrorMessage = "User data not found or does not exist.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        #endregion
    }
}
