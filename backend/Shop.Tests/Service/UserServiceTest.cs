using Moq;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Tests.Service
{
    public class UserServiceTest
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IUserAdditionalRepository> _mockUserAdditionalRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IJwtProvider> _mockJwtProvider;
        private readonly IUserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUserAdditionalRepository = new Mock<IUserAdditionalRepository>();
            _mockJwtProvider = new Mock<IJwtProvider>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();



            _userService = new UserService(
                _mockUserRepository.Object,
                _mockPasswordHasher.Object,
                _mockJwtProvider.Object,
                _mockUserAdditionalRepository.Object
                );
        }
        [Fact]
        public async Task Login_Should_ReturnToken_When_CredentialsAreVAlid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            var passwordHash = "hashedpassword";
            var user = new User
            {
                Email = email,
                PasswordHash = passwordHash,
            };

            _mockUserAdditionalRepository.Setup(repo => repo.GetByEmailAsync(email))
                .ReturnsAsync(user);

            _mockPasswordHasher.Setup(hasher => hasher.Verify(password, passwordHash))
                .Returns(true);

            _mockJwtProvider.Setup(provider => provider.GenerateToken(user))
                .Returns("mocked-jwt-token");

            // Act
            var token = await _userService.Login(email, password);

            //Assert

            Assert.Equal("mocked-jwt-token", token);
        }

        [Fact]
        public async Task Login_Should_ThrowException_When_PaawordIsIncorrect()
        {
            //Arrange
            var email = "test@example.com";
            var password = "wrongpassword";
            var user = new User
            {
                Email = email,
                PasswordHash = "hashepassword"
            };

            _mockUserAdditionalRepository.Setup(repo => repo.GetByEmailAsync(email))
                .ReturnsAsync(user);

            _mockPasswordHasher.Setup(hasher => hasher.Verify(password, user.PasswordHash))
                .Returns(false);

            //Act/Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.Login(email, password));
            Assert.Equal("Некорректный логин или пароль", exception.Message);
        }
        [Fact]
        public async Task Login_Should_ThrowException_When_UserNotFound()
        {
            //Arrange
            var email = "nonexisten@example.com";
            var password = "password";

            _mockUserAdditionalRepository.Setup(r => r.GetByEmailAsync(email))
                .ReturnsAsync((User)null);

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(()=> _userService.Login(email, password));
            Assert.Equal("Нет такого пользователя", exception.Message);

        }
    }
}
