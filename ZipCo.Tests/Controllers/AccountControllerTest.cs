using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;
using ZipCo.Data.Entities;
using ZipCo.WebAPI.Controllers;
using ZipCo.WebAPI.Services.Interface;
using ZipCo.WebAPI.ViewModels;

namespace ZipCo.Tests.Controllers
{
    public class AccountControllerTest
    {
        [Fact]
        public void ListAccount_ReturnsAcounts()
        {
            // Arrange
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            Mock<IService<AccountResponse, AccountRequest>> mockAccountService = new Mock<IService<AccountResponse, AccountRequest>>();

            List<AccountResponse> response = new List<AccountResponse>
            {
                new AccountResponse
                {
                    Account = new Account { AccountNumber = "123412341234" }
                }
            };

            mockAccountService.Setup(q => q.List()).Returns(response);

            AccountController controller = new AccountController(mockLogger.Object, mockAccountService.Object);

            // Act
            IActionResult result = controller.List();

            ObjectResult okObjectResult = result as ObjectResult;

            // Assert
            List<AccountResponse> actual = (List<AccountResponse>)okObjectResult.Value;

            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult is OkObjectResult);
            Assert.IsType<List<AccountResponse>>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(actual.Any());
        }

        [Fact]
        public void CreateAccount_ReturnsAccount()
        {
            // Arrange
            Mock<ILogger<AccountController>> mockLogger = new Mock<ILogger<AccountController>>();
            Mock<IService<AccountResponse, AccountRequest>> mockAccountService = new Mock<IService<AccountResponse, AccountRequest>>();

            User user = new User()
            {
                Name = "Allex Rocha",
                EmailAddress = "allexmmr@gmail.com",
                Password = "Izi5XK0sLgpoHc56Nisv",
                MonthlySalary = 2000,
                MonthlyExpenses = 1000
            };

            string accountNumber = "123412341234";

            AccountRequest request = new AccountRequest()
            {
                User = user,
                AccountNumber = accountNumber
            };

            AccountResponse response = new AccountResponse
            {
                Account = new Account() { User = user, AccountNumber = accountNumber },
                Success = true
            };

            mockAccountService.Setup(q => q.Create(request)).Returns(response);

            AccountController controller = new AccountController(mockLogger.Object, mockAccountService.Object);

            // Act
            IActionResult result = controller.Create(request);

            ObjectResult okObjectResult = result as ObjectResult;

            // Assert
            AccountResponse actual = (AccountResponse)okObjectResult.Value;

            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult is OkObjectResult);
            Assert.IsType<AccountResponse>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(actual.Success);
            Assert.Equal(accountNumber, actual.Account.AccountNumber);
        }
    }
}