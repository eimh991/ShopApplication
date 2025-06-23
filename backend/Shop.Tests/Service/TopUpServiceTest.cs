using Moq;
using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;

namespace Shop.Tests.Service
{
    public class TopUpServiceTest
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepositoryWithUser<BalanceHistory>> _balanceHistoryRepositoryMock;
        private readonly Mock<ITopUpCodeRepository> _topUpCodeRepositoryMock;
        private readonly Mock<IUserBalanceUpdater> _userBalanceUpdaterMock;
        private readonly TopUpService _topUpService;

        public TopUpServiceTest()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _balanceHistoryRepositoryMock = new Mock<IRepositoryWithUser<BalanceHistory>>();
            _topUpCodeRepositoryMock = new Mock<ITopUpCodeRepository>();
            _userBalanceUpdaterMock = new Mock<IUserBalanceUpdater>();

            _topUpService = new TopUpService(
                _topUpCodeRepositoryMock.Object,
                _userRepositoryMock.Object,
                _balanceHistoryRepositoryMock.Object,
                _userBalanceUpdaterMock.Object
                );
        }

        [Fact]
        public async Task CreateCodeAsync_WithValidIntegerAmount()
        {
            //Arrange
            decimal validAmount = 1000m; 
            var expectedCode = "TESTCODE123";

            _topUpCodeRepositoryMock
                .Setup(repo => repo.CreateNewCodeAsync(It.IsAny<TopUpAmount>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TopUpCode { Code = expectedCode });

            //Act
            var result = await _topUpService.CreateTopUpCodeAsync(validAmount, CancellationToken.None);

            //Assert
            Assert.Equal(expectedCode, result);
            _topUpCodeRepositoryMock.Verify(
                repo => repo.CreateNewCodeAsync((TopUpAmount)validAmount, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateTopUpCodeAsync_WithNonIntegerAmount_ThrowsArgumentException()
        {
            //Arrange
            decimal nonIntegerAmount = 99.99m;

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _topUpService.CreateTopUpCodeAsync(nonIntegerAmount, CancellationToken.None));
        }

        [Fact]
        public async Task CreateTopUpCodeAsync_WithInvalidIntegerAmount_ThrowsArgumentException()
        {
            //Arrange
            decimal invalidIntegerAmount = 999m; 

            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _topUpService.CreateTopUpCodeAsync(invalidIntegerAmount, CancellationToken.None));

            Assert.Contains($"Недопустимая сумма пополнения: {invalidIntegerAmount}", exception.Message);
        }

        [Fact]
        public async Task ApplyTopUpCodeAsync_ValidCodeAndUser_ReturnsTrueAndUpdatesBalance()
        {
            // Arrange
            var code = "VALIDCODE123";
            var userId = 1;
            var initialBalance = 1000m;
            var topUpAmount = TopUpAmount.Amount1000; 

            var user = new User { UserId = userId, Balance = initialBalance };
            var topUpCode = new TopUpCode { Code = code, Amount = topUpAmount };

            _topUpCodeRepositoryMock
                .Setup(x => x.GetValidCodeAsync(code, It.IsAny<CancellationToken>()))
                .ReturnsAsync(topUpCode);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _balanceHistoryRepositoryMock
                .Setup(x => x.AddAsync(userId, It.IsAny<BalanceHistory>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _userBalanceUpdaterMock
                .Setup(x => x.UpdateBalanceAsync(user, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _topUpService.ApplyTopUpCodeAsync(code, userId, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.Equal(initialBalance + (decimal)topUpAmount, user.Balance);

            _topUpCodeRepositoryMock.Verify(x => x.UseCodeAsync(topUpCode, userId, It.IsAny<CancellationToken>()), Times.Once);
            _balanceHistoryRepositoryMock.Verify(x => x.AddAsync(userId, It.Is<BalanceHistory>(h =>
                h.Amount == (decimal)topUpAmount &&
                h.Description.Contains(code)), It.IsAny<CancellationToken>()), Times.Once);
            _userBalanceUpdaterMock.Verify(x => x.UpdateBalanceAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
