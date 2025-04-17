using Moq;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using StackExchange.Redis;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Tests.Service
{
    public  class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_Should_SaveOrder_And_DeleteCart_AndClearCache()
        {
            //Arrange
            var mockOrderRepo = new Mock<IRepositoryWithUser<Model.Order>>();
            var mockCartRepo = new Mock<IRepositoryWithUser<CartItem>>();
            var mockRedisDb = new Mock<StackExchange.Redis.IDatabase>();

            var userId = 1;
            var cartItems = new List<CartItem>()
            {
                new CartItem
                {
                    Quantity = 2,
                    Product = new Product
                    {
                        Name = "Test Product",
                        Price = 10m,
                    }
                }
            };

            // Настроим моки так, чтобы они не выбрасывали исключения
            mockOrderRepo.Setup(r => r.AddAsync(userId, It.IsAny<Model.Order>())).Returns(Task.CompletedTask);
            mockCartRepo.Setup(r => r.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            mockRedisDb.Setup(db => db.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(Task.FromResult(true));

            var orderService = new OrderService(mockOrderRepo.Object, mockCartRepo.Object, mockRedisDb.Object);

            // Act
            await orderService.CreateOrderAsync(userId, cartItems);

            // Assert
            mockOrderRepo.Verify(r => r.AddAsync(userId, It.IsAny<Model.Order>()), Times.Once());
            mockCartRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Exactly(cartItems.Count));  // Проверяем, что для каждого элемента корзины был вызван метод удаления
            mockRedisDb.Verify(db => db.KeyDeleteAsync(It.Is<string>(key => key == $"order_user{userId}"), CommandFlags.None), Times.Once);
        }
    }
}
