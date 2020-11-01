using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using ZipCo.Data.Entities;
using ZipCo.Data.Repositories.Interfaces;
using ZipCo.WebAPI.Services.Interface;
using ZipCo.WebAPI.ViewModels;

namespace ZipCo.WebAPI.Services
{
    /// <summary>
    /// Account Service Class.
    /// </summary>
    public class AccountService : IService<AccountResponse, AccountRequest>
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IRepository<Account> _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Account Service Constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="accountRepository">Account Repository</param>
        /// <param name="userRepository">User Repository</param>
        /// <param name="configuration">Configuration</param>
        public AccountService(ILogger<AccountService> logger, IRepository<Account> accountRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// List all accounts.
        /// </summary>
        /// <returns>Return a list of accounts.</returns>
        public IEnumerable<AccountResponse> List()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(List));

            try
            {
                IEnumerable<Account> accounts = _accountRepository.List();

                return accounts.Select(x => new AccountResponse
                {
                    Account = new Account
                    {
                        Id = x.Id,
                        UserId = x.User.Id,
                        AccountNumber = x.AccountNumber
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
        /// Create an account.
        /// </summary>
        /// <param name="request">Account request</param>
        /// <returns>Return the created account.</returns>
        public AccountResponse Create(AccountRequest request)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(Create));

            try
            {
                AccountResponse response = null;

                if (!IsModelValid(request, ref response)
                    || !IsUserFound(request.User, ref response))
                {
                    return response;
                }

                User user = _userRepository.Get(request.User.EmailAddress);

                if (user == null)
                {
                    // Create new user if not found
                    user = _userRepository.Create(request.User);
                }

                if (IsAccountFound(request, ref response)
                    || !IsUserEligible(user, GetMinAllowedCredit(), ref response))
                {
                    return response;
                }

                Account account = _accountRepository.Create(new Account
                {
                    User = user,
                    AccountNumber = request.AccountNumber
                });

                response.Account = account;
                response.Account.Id = account.Id;
                response.Success = true;

                _logger?.LogDebug("Account '{0}' has been created.", request.AccountNumber);

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(Create), ex);

                return new AccountResponse { ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Get the minimum allowed credit as per business requirement.
        /// </summary>
        /// <returns>Return the minimum allowed credit.</returns>
        protected decimal GetMinAllowedCredit()
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(GetMinAllowedCredit));

            return _configuration != null ? _configuration.GetValue<decimal>("BusinessRequirements:MinAllowedCredit") : 1000;
        }

        #region Validations

        private bool IsModelValid(AccountRequest request, ref AccountResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsModelValid));

            if (request == null)
            {
                response = new AccountResponse
                {
                    ErrorMessage = "Account data not found.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        private bool IsUserFound(User user, ref AccountResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsUserFound));

            if (user == null)
            {
                response = new AccountResponse
                {
                    ErrorMessage = "User not found or does not exist.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        private bool IsAccountFound(AccountRequest request, ref AccountResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsAccountFound));

            Account account = _accountRepository.List().FirstOrDefault(x => x.User.EmailAddress == request.User.EmailAddress && x.AccountNumber == request.AccountNumber);

            if (account != null)
            {
                response = new AccountResponse
                {
                    ErrorMessage = "This user already has an account.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        private bool IsUserEligible(User user, decimal minAllowedCredit, ref AccountResponse response)
        {
            _logger?.LogDebug("'{0}' has been invoked.", nameof(IsUserEligible));

            if (user == null || !(user.MonthlySalary - user.MonthlyExpenses >= minAllowedCredit))
            {
                response = new AccountResponse
                {
                    ErrorMessage = "User is not eligible to create an account.",
                    Success = false
                };

                return false;
            }

            return true;
        }

        #endregion
    }
}