using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;
using ZipCo.Data.Entities;
using ZipCo.Data.Repositories.Interfaces;
using ZipCo.WebAPI.Services;
using ZipCo.WebAPI.ViewModels;

namespace ZipCo.Tests.Services
{
    public class AccountServiceTest
    {
        private readonly AccountService _accountService;
        private readonly AccountRequest _request;

        public AccountServiceTest()
        {
            // Arrange
            Mock<ILogger<AccountService>> mockLogger = new Mock<ILogger<AccountService>>();
            Mock<IRepository<Account>> mockAccountRepository = new Mock<IRepository<Account>>();
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            _accountService = new AccountService(mockLogger.Object, mockAccountRepository.Object, mockUserRepository.Object, mockConfiguration.Object);
            _request = new AccountRequest()
            {
                User = new User
                {
                    Name = "Allex Rocha",
                    EmailAddress = "allexmmr@gmail.com",
                    Password = "Izi5XK0sLgpoHc56Nisv",
                    MonthlySalary = 2000,
                    MonthlyExpenses = 1000
                },
                AccountNumber = "123412341234"
            };
        }

        [Fact]
        public void ListAccounts_CheckIfAny()
        {
            // Act
            IEnumerable<AccountResponse> actual = _accountService.List();

            // Assert
            Assert.NotNull(actual);
            //Assert.True(actual.Any());
        }
    }
}