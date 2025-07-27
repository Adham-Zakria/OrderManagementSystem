using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Moq;
using Services;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TestProject
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
        private readonly Mock<IConfiguration> _mockConfig = new();
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("supersecretkeysupersecretkey123456");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            _userService = new UserService(_mockUnitOfWork.Object, _mockConfig.Object);
        }
        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenUsernameAlreadyExists()
        {
            _mockUnitOfWork.Setup(x => x.UserRepository.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(new User());

            var dto = new RegisterUserDto { Username = "existing", Password = "123", Role = "User" };

            await Assert.ThrowsAsync<AppException>(() => _userService.RegisterAsync(dto));
        }

        [Fact]
        public async Task LoginAsync_ShouldThrow_WhenCredentialsInvalid()
        {
            _mockUnitOfWork.Setup(x => x.UserRepository.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            var dto = new LoginRequestDto { Username = "baduser", Password = "badpass" };

            await Assert.ThrowsAsync<AppException>(() => _userService.LoginAsync(dto));
        }
    }
}
