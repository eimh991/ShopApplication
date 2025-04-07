using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Shop.Service
{
    public class OrderService : IOrderService
    {

        private readonly IRepositoryWithUser<Model.Order> _orderRepository;
        private readonly IRepositoryWithUser<CartItem> _cartItemRepository;
        private readonly IDatabase _redisDb;

        public OrderService(IRepositoryWithUser<Model.Order> orderRepository,IRepositoryWithUser<CartItem> cartItemRepository, IDatabase redisDb)
        {
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
            _redisDb = redisDb;
        }

        public async Task CreateOrderAsync(int userId,IEnumerable<CartItem> items)
        {
            var orderItems = ConvertCartItemsToOrderItems(items);
            var order = new Model.Order
            {
                OrderItems = orderItems.ToList(),
                OrderDate = DateTime.Now,
                TotalAmount = AllOrderPrice(orderItems.ToList())
            };
            await _orderRepository.AddAsync(userId, order);
            await ((CartItemRepository)_cartItemRepository).DeleteAllCartItemsAsync(userId);

            // ⛔ Очистка кэша для этого пользователя
            await _redisDb.KeyDeleteAsync($"orders_user_{userId}");
        }

        public async Task<IEnumerable<Model.Order>> GetAllOrdersAsync(int userId)
        {
            // Ключ для Redis
            var cacheKey = $"orders_user_{userId}";

            // Пытаемся получить кэшированные данные
            var cachedOrders = await _redisDb.StringGetAsync(cacheKey);

            if (!cachedOrders.IsNullOrEmpty)
            {
                // Если данные есть в кэше, десериализуем их и возвращаем
                return JsonConvert.DeserializeObject<List<Model.Order>>(cachedOrders);
            }

            var ordersFromDb = await _orderRepository.GetAllAsync(userId);

            // Если данные есть, сериализуем и сохраняем в Redis с TTL
            if (ordersFromDb != null && ordersFromDb.Any())
            {
                await _redisDb.StringSetAsync(cacheKey,JsonConvert.SerializeObject(ordersFromDb),TimeSpan.FromMinutes(10)); // Устанавливаем срок жизни кэша (10 минут)

            }

            // Возвращаем данные из базы
            return ordersFromDb ?? Enumerable.Empty<Model.Order>();

        }

        public async Task<Model.Order> GetOrderByIdAsync(int userId, int entityId)
        {
            return await _orderRepository.GetByIdAsync(userId, entityId);
        }

        public async Task DeleteOrderAsync(int entityId)
        {
             await _orderRepository.DeleteAsync(entityId);

        }

        private IEnumerable<OrderItem> ConvertCartItemsToOrderItems(IEnumerable<CartItem> items)
        {
            return items.Select(i => new OrderItem
            {
                Product = i.Product,
                Quantity = i.Quantity,
                UnitPrice = i.Quantity * i.Product.Price,
            });
        }

        private decimal AllOrderPrice(List<OrderItem> orderItems)
        {
            decimal allPrice = 0;
            for (int i = 0; i < orderItems.Count; i++)
            {
                allPrice += orderItems[i].UnitPrice;
            }
            return allPrice;
        }

    }
}
