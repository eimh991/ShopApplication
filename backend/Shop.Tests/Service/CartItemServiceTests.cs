using Moq;
using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;

namespace Shop.Tests.Service
{
    public class CartItemServiceTests
    {
        private readonly Mock<IRepositoryWithUser<CartItem>> _cartItemRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IUserAdditionalRepository> _additionalRepositoryMock;
        private readonly Mock<ICartItemCleaner> _cartItemCleanerMock;
        private readonly Mock<ICartItemExtendedRepository> _cartItemExtendedRepositoryMock;
        private readonly CartItemSercive _cartItemSercive;

        public CartItemServiceTests()
        {
            _cartItemRepositoryMock = new Mock<IRepositoryWithUser<CartItem>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _additionalRepositoryMock = new Mock<IUserAdditionalRepository>();
            _cartItemCleanerMock = new Mock<ICartItemCleaner>();
            _cartItemExtendedRepositoryMock = new Mock<ICartItemExtendedRepository>();

            _cartItemSercive = new CartItemSercive(
                    _cartItemRepositoryMock.Object,
                    _userRepositoryMock.Object,
                    _productRepositoryMock.Object,
                    _additionalRepositoryMock.Object,
                    _cartItemCleanerMock.Object,
                    _cartItemExtendedRepositoryMock.Object
                );
        }

        [Fact]
        public async Task ClearAllCartItemsAsync_Shoild_Invoke_DeleteAllCartItemsAsync()
        {
            //Arrange
            int userId = 1;
            _cartItemCleanerMock.Setup(cl=>cl.DeleteAllCartItemsAsync(userId)).Returns(Task.CompletedTask);

            //Act
            await _cartItemSercive.ClearAllCartItemsAsync(userId);

            //Assert

            _cartItemCleanerMock.Verify(cl=>cl.DeleteAllCartItemsAsync(userId), Times.Once());
        }

        [Fact]
        public async Task CreateCartItemAsync_ShouldReturnCartItemId()
        {
            //Arrange
            var dto = new CartItemDTO { UserId = 1, ProductId = 2, Quantity = 3 };
            var product = new Product { ProductId = 2 };
            var user = new User
            {
                Cart = new Cart()
                { CartId = 10 }
            };

            _productRepositoryMock.Setup(pr=>pr.GetByIdAsync(dto.ProductId))
                .ReturnsAsync(product);

            _additionalRepositoryMock.Setup(addRep=>addRep.GetUserWithCartAsync(dto.UserId))
                .ReturnsAsync(user);

            _cartItemExtendedRepositoryMock.Setup(ext => ext.AddAsyncAndReturnCartItemId(dto.UserId, It.IsAny<CartItem>()))
               .ReturnsAsync(42);

            //Act
            var result = await _cartItemSercive.CreateCartItemAsync(dto);

            //Assert
            Assert.Equal(42, result);

            _cartItemExtendedRepositoryMock.Verify(ext=>ext.AddAsyncAndReturnCartItemId(dto.UserId,It.IsAny<CartItem>())
                ,Times.Once());
        }

        [Fact]
        public async Task DeleteCartItemsAsync_ShouldCallRepositoryDelete()
        {
            //Arrange
            int cartItemId = 5;

            _cartItemRepositoryMock.Setup(r=>r.DeleteAsync(cartItemId))
                .Returns(Task.CompletedTask);

            //Act
            await _cartItemSercive.DeleteCartItemAsync(cartItemId);

            //Assert
            _cartItemRepositoryMock.Verify(r=>r.DeleteAsync(cartItemId), Times.Once()); 
        }

        [Fact]
        public async Task GetAllCartItemsAsync_ShouldReturnMeppedDTOs()
        {
            //Arrange
            int userId = 1;
            var cartItems = new List<CartItem>()
            {
                new CartItem
                {
                    CartItemId = 1,
                    Quantity = 2,
                    ProductId = 3,
                    CartId = 4
                }
            };

            _cartItemRepositoryMock.Setup(r=>r.GetAllAsync(userId))
                .ReturnsAsync(cartItems);

            //Act
            var result = await _cartItemSercive.GetAllCartItemsAsync(userId);

            //Assert

            Assert.Single(result);
            Assert.Equal(1, result.First().CartItemId);
        }

        [Fact]
        public async Task UpdateCountCartItemsAsync_ShouldCallUpdateQuantity()
        {
            //Arrange
            int id = 10, quantity = 5;
            _cartItemExtendedRepositoryMock.Setup(ext => ext.UpdateQuantityAsync
                                (It.Is<CartItem>(c=>c.CartItemId == id && c.Quantity == quantity)))
                                .Returns(Task.CompletedTask);

            //Act
            await _cartItemSercive.UpdateCountCartItemsAsync(id, quantity);

            //Assert
            _cartItemExtendedRepositoryMock.Verify(ext => ext.UpdateQuantityAsync(It.IsAny<CartItem>()), Times.Once);
                                   
        }

        [Fact]
        public async Task GetAllCartProductAsync_ShouldReturnCartProducts()
        {
            //Arrange
            int userId = 1;
            var expectedList = new List<CartProductDTO> { new CartProductDTO { } };
            _cartItemExtendedRepositoryMock.Setup(ext => ext.GetAllCartProductAsync(userId))
                                .ReturnsAsync(expectedList);

            //Act
            var result = await _cartItemSercive.GetAllCartProductAsync(userId);

            //Assert
            Assert.Single(result);
        }

    }
}
