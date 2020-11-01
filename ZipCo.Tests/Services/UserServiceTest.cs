using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZipCo.Data.Repositories.Interfaces;
using ZipCo.WebAPI.Services;
using ZipCo.WebAPI.ViewModels;

namespace ZipCo.Tests.Services
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly UserRequest _request;

        public UserServiceTest()
        {
            // Arrange
            Mock<ILogger<UserService>> mockLogger = new Mock<ILogger<UserService>>();
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(mockLogger.Object, mockUserRepository.Object);
            _request = new UserRequest()
            {
                Name = "Allex Rocha",
                EmailAddress = "allexmmr@gmail.com",
                Password = "Izi5XK0sLgpoHc56Nisv",
                MonthlySalary = 2000,
                MonthlyExpenses = 1000
            };
        }

        [Fact]
        public void ListUsers_CheckIfAny()
        {
            // Act
            IEnumerable<UserResponse> actual = _userService.List();

            // Assert
            Assert.NotNull(actual);
            //Assert.True(actual.Any());
        }
    }
}