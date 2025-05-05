using Moq;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using Shop.UsersDTO;
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

            _mockUserAdditionalRepository.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mockPasswordHasher.Setup(hasher => hasher.Verify(password, passwordHash))
                .Returns(true);

            _mockJwtProvider.Setup(provider => provider.GenerateToken(user))
                .Returns("mocked-jwt-token");

            // Act
            var token = await _userService.Login(email, password, CancellationToken.None);

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

            _mockUserAdditionalRepository.Setup(repo => repo.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _mockPasswordHasher.Setup(hasher => hasher.Verify(password, user.PasswordHash))
                .Returns(false);

            //Act/Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.Login(email, password, It.IsAny<CancellationToken>()));
            Assert.Equal("Некорректный логин или пароль", exception.Message);
        }
        [Fact]
        public async Task Login_Should_ThrowException_When_UserNotFound()
        {
            //Arrange
            var email = "nonexisten@example.com";
            var password = "password";

            _mockUserAdditionalRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(()=> _userService.Login(email, password, It.IsAny<CancellationToken>()));
            Assert.Equal("Нет такого пользователя", exception.Message);

        }

        [Fact]

        public async Task ChangeStatusAsync_Should_CallChangeStatus_When_ValidStatus()
        {
            //Arrange
            var userId = 1;
            var status = "Manager";
            _mockUserAdditionalRepository.Setup(r=>r.ChangeStatusAsync(userId, status, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //Act
            await _userService.ChangeStatusAsync(userId, status, CancellationToken.None);

            //Assert
            _mockUserAdditionalRepository.Verify(r=>r.ChangeStatusAsync(userId,status, It.IsAny<CancellationToken>()), Times.Once());  
        }

        [Fact]
        public async Task ChangeStatusAsync_Should_NotCallChangeStatus_When_InvalidStatus()
        {
            //Arrange
            var userId = 1;
            var invalidstatus = "UnCorrectStatus";

            //Act
            await _userService.ChangeStatusAsync(userId, invalidstatus, CancellationToken.None);

            //Assert
            _mockUserAdditionalRepository.Verify(r=>r.
                            ChangeStatusAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),Times.Never());

        }

        [Fact]

        public async Task CreateAsync_Should_CreateUser()
        {
            //Arrange
            var userDTO = new UserDTO()
            {
                UserName = "New User",
                Email = "newuser@example.com",
                Password = "password123",
            };
            
            //Act
            await _userService.CreateAsync(userDTO, CancellationToken.None);

            //Assert
            _mockUserRepository.Verify(r=>r.CreateAsync(It.IsAny<User>(),It.IsAny<CancellationToken>()), Times.Once());


        }


        [Fact]
        public async Task DeleteAsync_Should_DeleteUser()
        {
            //Arrange
            var userId = 1;

            _mockUserRepository.Setup(r=>r.DeleteAsync(userId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            //Act
            await _userService.DeleteAsync(userId, CancellationToken.None);

            ////Assert
            _mockUserRepository.Verify(r=>r.DeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once());

        }

        [Fact] 
        public async Task GetByIdAsync_Should_ReturnUser_When_UserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User()
            {
                UserId = userId,
                UserName = "Test user",
                Email = "test@example.com",
            };

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            //Act
            var result = await _userService.GetByIdAsync(userId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNull_When_UserNotExists()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            //Act
            var result = await _userService.GetByIdAsync(userId, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmaiAsync_Should_ReturnUser_When_UserExists()
        {
            //Arrange
            var email = "test@example.com";
            var user = new User()
            {
                Email = email,
                UserName = "New user"
            };

            _mockUserAdditionalRepository.Setup(r=>r.GetByEmailAsync(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            //Act 
            var result = await _userService.GetByEmaiAsync(email, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetByEmaiAsync_Should_ReturnNull_When_UserDoesNotExist()
        {
            //Arrange
            var email = "noneexist@example.com";

            _mockUserAdditionalRepository.Setup(r => r.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            //Act
            var result = await _userService.GetByEmaiAsync(email, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateUser()
        {
            // Arrange
            var userDto = new UserDTO 
            {
                UserId = 1,
                UserName = "Updated User",
                Email = "updated@example.com",
                Password = "newpassword123"
            };

            _mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>(),It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _userService.UpdateAsync(userDto, CancellationToken.None);

            // Assert
            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetUserCartItemsAsync_Should_ReturnCartItems_When_UserExists()
        {
            // Arrange
            var userId = 1;
            var cartItems = new List<CartItem> 
            { new CartItem 
                { 
                    ProductId = 1, 
                    Quantity = 2 
                }
            };

            var user = new User
            {
                UserId = userId,
                Cart = new Cart
                {
                    CartItems = cartItems
                }
            };

            _mockUserAdditionalRepository.Setup(r=>r.GetUserWithCartsItemAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            //Act
            var result = await _userService.GetUserCartItemsAsync(userId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(cartItems.Count, result.Count());
        }

        [Fact]
        public async Task GetUserCartItemsAsync_Should_ReturnEmpty_When_UserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockUserAdditionalRepository.Setup(r => r.GetUserWithCartsItemAsync(userId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserCartItemsAsync(userId, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }


    }
}
