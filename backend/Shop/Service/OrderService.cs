using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;
using Newtonsoft.Json;
using StackExchange.Redis;
using Shop.DTO;

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
                OrderDate = DateTime.UtcNow,
                TotalAmount = AllOrderPrice(orderItems.ToList())
            };
            await _orderRepository.AddAsync(userId, order);
            await ((CartItemRepository)_cartItemRepository).DeleteAllCartItemsAsync(userId);

            //  Очистка кэша для этого пользователя
            await _redisDb.KeyDeleteAsync($"orders_user_{userId}");
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersDTOAsync(int userId)
        {
            var cacheKey = $"orders_user_{userId}";

            var cachedOrders = await _redisDb.StringGetAsync(cacheKey);

            if (!cachedOrders.IsNullOrEmpty)
            {
               
                return JsonConvert.DeserializeObject<List<OrderDTO>>(cachedOrders);
            }

            var ordersFromDb = await _orderRepository.GetAllAsync(userId);
            var orderDTO = ConverOrderToOrderDTO(ordersFromDb);

            // Если данные есть, сериализуем и сохраняем в Redis
            if (orderDTO != null && orderDTO.Any())
            {
                
                await _redisDb.StringSetAsync(cacheKey,JsonConvert.SerializeObject(orderDTO),TimeSpan.FromMinutes(10)); 

            }
            return orderDTO ?? Enumerable.Empty<OrderDTO>();

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
            if (items == null)
                throw new ArgumentNullException(nameof(items), "Cart items list is null");

            return items.Select(i =>
            {
                if (i == null)
                    throw new NullReferenceException("CartItem is null");

                if (i.Product == null)
                    throw new NullReferenceException("CartItem.Product is null");

                return new OrderItem
                {
                    Product = i.Product,
                    Quantity = i.Quantity,
                    UnitPrice = i.Quantity * (i.Product?.Price ?? 0),
                };
            });

            /*
             return items.Select(i => new OrderItem
             {
                 Product = i.Product,
                 Quantity = i.Quantity,
                 UnitPrice = i.Quantity * i.Product.Price,
             });
            */
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

        private IEnumerable<OrderDTO> ConverOrderToOrderDTO(IEnumerable<Model.Order> order)
        {
           var orderDTO = order.Select(order => new OrderDTO
                {
                        OrderDate = order.OrderDate,
                        TotalAmount = order.TotalAmount,
                        OrderId = order.OrderId,
                        orderItemDTOs = order.OrderItems.Select(item => new OrderItemDTO{
                            OrderItemId = item.OrderItemId,
                            Product = item.Product,
                            Quantity = item.Quantity,   
                    
                        }).ToList(),
                    }).ToList();

            return orderDTO;
                    
        }

        Task<IEnumerable<Model.Order>> IOrderService.GetAllOrdersAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
