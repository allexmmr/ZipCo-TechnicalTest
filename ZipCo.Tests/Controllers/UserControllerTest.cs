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
    public class UserControllerTest
    {
        [Fact]
        public void ListUser_ReturnsUsers()
        {
            // Arrange
            Mock<ILogger<UserController>> mockLogger = new Mock<ILogger<UserController>>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();

            List<UserResponse> response = new List<UserResponse>
            {
                new UserResponse
                {
                    User = new User
                    {
                        Name = "Allex Rocha",
                        EmailAddress = "allexmmr@gmail.com",
                        Password = "Izi5XK0sLgpoHc56Nisv",
                        MonthlySalary = 2000,
                        MonthlyExpenses = 1000
                    }
                }
            };

            mockUserService.Setup(q => q.List()).Returns(response);

            UserController controller = new UserController(mockLogger.Object, mockUserService.Object);

            // Act
            IActionResult result = controller.List();

            ObjectResult okObjectResult = result as ObjectResult;

            // Assert
            List<UserResponse> actual = (List<UserResponse>)okObjectResult.Value;

            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult is OkObjectResult);
            Assert.IsType<List<UserResponse>>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(actual.Any());
        }

        [Fact]
        public void CreateUser_ReturnsUser()
        {
            // Arrange
            Mock<ILogger<UserController>> mockLogger = new Mock<ILogger<UserController>>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();

            UserRequest request = new UserRequest()
            {
                Name = "Allex Rocha",
                EmailAddress = "allexmmr@gmail.com",
                Password = "Izi5XK0sLgpoHc56Nisv",
                MonthlySalary = 2000,
                MonthlyExpenses = 1000
            };

            UserResponse response = new UserResponse
            {
                User = new User()
                {
                    Id = 2,
                    Name = "Allex Rocha",
                    EmailAddress = "allexmmr@gmail.com",
                    Password = "Izi5XK0sLgpoHc56Nisv",
                    MonthlySalary = 2000,
                    MonthlyExpenses = 1000
                },
                Success = true
            };

            mockUserService.Setup(q => q.Create(request)).Returns(response);

            UserController controller = new UserController(mockLogger.Object, mockUserService.Object);

            // Act
            IActionResult result = controller.Create(request);

            ObjectResult okObjectResult = result as ObjectResult;

            // Assert
            UserResponse actual = (UserResponse)okObjectResult.Value;

            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult is OkObjectResult);
            Assert.IsType<UserResponse>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(actual.Success);
            Assert.Equal("Allex Rocha", actual.User.Name);
        }

        [Fact]
        public void GetUser_ReturnsUser()
        {
            // Arrange
            Mock<ILogger<UserController>> mockLogger = new Mock<ILogger<UserController>>();
            Mock<IUserService> mockUserService = new Mock<IUserService>();

            string emailAddress = "allexmmr@gmail.com";

            UserResponse response = new UserResponse
            {
                User = new User()
                {
                    Id = 2,
                    Name = "Allex Rocha",
                    EmailAddress = "allexmmr@gmail.com",
                    Password = "Izi5XK0sLgpoHc56Nisv",
                    MonthlySalary = 2000,
                    MonthlyExpenses = 1000
                },
                Success = true
            };

            mockUserService.Setup(q => q.Get(emailAddress)).Returns(response);

            UserController controller = new UserController(mockLogger.Object, mockUserService.Object);

            // Act
            IActionResult result = controller.Get(emailAddress);

            ObjectResult okObjectResult = result as ObjectResult;

            // Assert
            UserResponse actual = (UserResponse)okObjectResult.Value;

            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult is OkObjectResult);
            Assert.IsType<UserResponse>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(actual.Success);
            Assert.Equal("Allex Rocha", actual.User.Name);
        }
    }
}