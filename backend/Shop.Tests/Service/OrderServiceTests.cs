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
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_Should_SaveOrder_And_DeleteCart_AndClearCache()
        {
            //Arrange
            var mockOrderRepo = new Mock<IRepositoryWithUser<Model.Order>>();
            var mockCartRepo = new Mock<ICartItemCleaner>();
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
            mockCartRepo.Setup(r => r.DeleteAllCartItemsAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            mockRedisDb.Setup(db => db.KeyDeleteAsync(It.Is<RedisKey>(key => key == new RedisKey($"order_user_{userId}")), CommandFlags.None))
                .Returns(Task.FromResult(true)); // Возвращаем Task<bool> с результатом true

            var orderService = new OrderService(mockOrderRepo.Object, mockCartRepo.Object, mockRedisDb.Object);

            // Act
            await orderService.CreateOrderAsync(userId, cartItems);

            // Assert
            mockOrderRepo.Verify(r => r.AddAsync(userId, It.IsAny<Model.Order>()), Times.Once());
            mockCartRepo.Verify(r => r.DeleteAllCartItemsAsync(userId), Times.Once());
            mockRedisDb.Verify(db => db.KeyDeleteAsync(new RedisKey($"order_user_{userId}"), CommandFlags.None), Times.Once);
        }
    }
}