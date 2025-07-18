﻿using Moq;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using StackExchange.Redis;
using System.ComponentModel;
using System.Text.Json;
using Xunit;


namespace Shop.Tests.Service
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepositoryWithUser<Model.Order>> _mockOrderRepo;
        private readonly Mock<ICartItemCleaner> _mockCartItemCleanerRepo;
        private readonly Mock<IRepository<User>> _mockUserRepo;
        private readonly Mock<IUserBalanceUpdater> _mockUserBalanceUpdater;
        private readonly Mock<StackExchange.Redis.IDatabase> _mockRedisDb;
        private readonly Mock<IRepositoryWithUser<BalanceHistory>> _mockBalanceHistoryRepo;
        private readonly OrderService _orderService;
        

        public OrderServiceTests()
        {
            _mockOrderRepo = new Mock<IRepositoryWithUser<Model.Order>>();
            _mockUserRepo = new Mock<IRepository<User>>();
            _mockCartItemCleanerRepo = new Mock<ICartItemCleaner>();
            _mockUserBalanceUpdater = new Mock<IUserBalanceUpdater>();
            _mockBalanceHistoryRepo = new Mock<IRepositoryWithUser<BalanceHistory>>();
            _mockRedisDb = new Mock<IDatabase>();

            _orderService = new OrderService(
                _mockOrderRepo.Object,
                _mockCartItemCleanerRepo.Object,
                _mockUserRepo.Object,
                _mockUserBalanceUpdater.Object,
                _mockRedisDb.Object,
                _mockBalanceHistoryRepo.Object
             );
            

        }

        private List<CartItem> GetSampleCartItems() {
            {
                return new List<CartItem>()
                {
                    new CartItem
                    {
                        Quantity = 2,
                        Product = new Product
                        {
                            ProductId = 1,
                            Name = "Sample Product",
                            Price = 15.5m,
                            ImagePath = "default.jpg"
                        }
                    }

                };
               
            };
        }
        private List<Model.Order> GetSampleOrders()
        {
            return new List<Model.Order> {

                new Model.Order
                {
                    OrderId = 1,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 100m,
                    UserId = 1
                }
            };
             
        }


        [Fact]
        public async Task CreateOrderAsync_Should_SaveOrder_And_DeleteCart_AndClearCache()
        {
            //Arrange
            var userId = 1;
            var cartItems = GetSampleCartItems();
            var user = new User { UserId = userId, Balance = 1000 };


            _mockUserRepo.Setup(r=>r.GetByIdAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _mockOrderRepo.Setup(r => r.AddAsync(userId, It.IsAny<Model.Order>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockCartItemCleanerRepo.Setup(r => r.DeleteAllCartItemsAsync(userId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUserBalanceUpdater.Setup(u=>u.UpdateBalanceAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockBalanceHistoryRepo.Setup(b => b.AddAsync(userId, It.IsAny<BalanceHistory>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockRedisDb.Setup(db => db.KeyDeleteAsync(
                It.Is<RedisKey>(key => key.ToString() == $"order_user_{userId}"), CommandFlags.None))
                .ReturnsAsync(true); 

            // Act
            await _orderService.CreateOrderAsync(userId, cartItems, CancellationToken.None);

            // Assert
            _mockUserRepo.Verify(r=>r.GetByIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            _mockOrderRepo.Verify(r => r.AddAsync(userId, It.IsAny<Model.Order>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockCartItemCleanerRepo.Verify(r => r.DeleteAllCartItemsAsync(userId, It.IsAny<CancellationToken>()), Times.Once());
            _mockUserBalanceUpdater.Verify(u => u.UpdateBalanceAsync(It.Is<User>(u => u.UserId == userId), It.IsAny<CancellationToken>()), Times.Once());
            _mockBalanceHistoryRepo.Verify(bh=>bh.AddAsync(userId, It.IsAny<BalanceHistory>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockRedisDb.Verify(db => db.KeyDeleteAsync(It.IsAny<RedisKey>(), CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task GetAllOrdersDTOAsync_Should_ReturnOrders_FromCache_When_CacheExist()
        {
            //Arrange
            var userId = 1;
            var orders = GetSampleOrders();
            var  serializeOrders = JsonSerializer.Serialize(orders);

            _mockRedisDb.Setup(db=> db.StringGetAsync(
                    It.IsAny<RedisKey>(),CommandFlags.None))
                .ReturnsAsync(serializeOrders);

            //Act
            var result = await _orderService.GetAllOrdersDTOAsync(userId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(orders[0].OrderId, result.ToList()[0].OrderId); 
        }

        [Fact]
        public async Task GetOrderByIdAsync_Should_ReturnOrder_When_OrderExists()
        {
            //Arrange
            var userId = 1;
            var orderId = 1;
            var order = GetSampleOrders().FirstOrDefault();

            _mockOrderRepo.Setup(r=>r.GetByIdAsync(userId, orderId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(order);

            //Act
            var result = await _orderService.GetOrderByIdAsync(userId,orderId, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);
        }

        [Fact]
        public async Task DeleteOrderAsync_Should_DeleteOrder_AndClearCache()
        {
            //Arrange
            var orderId = 1;

            _mockOrderRepo.Setup(r=>r.DeleteAsync(orderId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //Act
            await _orderService.DeleteOrderAsync(orderId, CancellationToken.None);

            //Assert
            _mockOrderRepo.Verify(r=>r.DeleteAsync(orderId, It.IsAny<CancellationToken>()), Times.Once());            
        }
        

    }
}