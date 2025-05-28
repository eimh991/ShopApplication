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
        private readonly IRepository<User> _userRepository;
        private readonly ICartItemCleaner _cartCleaner;
        private readonly IUserBalanceUpdater _userBalanceUpdater;
        private readonly IDatabase _redisDb;
        private readonly IRepositoryWithUser<BalanceHistory> _balanceHistoryRepository;

        public OrderService(IRepositoryWithUser<Model.Order> orderRepository, ICartItemCleaner cartCleaner,
            IRepository<User> userRepository, IUserBalanceUpdater userBalanceUpdater,IDatabase redisDb, IRepositoryWithUser<BalanceHistory> balanceHistoryRepository)
        {
            _orderRepository = orderRepository;
            _cartCleaner = cartCleaner;
            _userBalanceUpdater= userBalanceUpdater;    
            _userRepository = userRepository;
            _redisDb = redisDb;
            _balanceHistoryRepository = balanceHistoryRepository;
        }

        public async Task CreateOrderAsync(int userId,IEnumerable<CartItem> items, CancellationToken cancellationToken)
        {
            var orderItems = ConvertCartItemsToOrderItems(items);
            var order = new Model.Order
            {
                OrderItems = orderItems.ToList(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = AllOrderPrice(orderItems.ToList())
            };
            
            var user = await _userRepository.GetByIdAsync(userId,cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException($"Пользователь с id {userId} не найден.");
            }
            if (user.Balance < order.TotalAmount)
            {
                throw new InvalidOperationException("Недостаточно средств для оформления заказа.");
            }
            await _orderRepository.AddAsync(userId, order,cancellationToken);
            user.Balance -= order.TotalAmount;
            
            await _userBalanceUpdater.UpdateBalanceAsync(user, cancellationToken);

            var history = new BalanceHistory
            {
                Amount = -order.TotalAmount,
                Description = $"Вы оформили заказа {order.OrderId}  и у вас с баланса списали {order.TotalAmount}",
                TransactionDate = DateTime.UtcNow,
                UserId = userId
            };
            await _balanceHistoryRepository.AddAsync(userId, history, cancellationToken);

            await _cartCleaner.DeleteAllCartItemsAsync(userId, cancellationToken);

            //  Очистка кэша для этого пользователя
            await _redisDb.KeyDeleteAsync($"order_user_{userId}", CommandFlags.None);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersDTOAsync(int userId, CancellationToken cancellationToken)
        {
            var cacheKey = $"order_user_{userId}";

            var cachedOrders = await _redisDb.StringGetAsync(cacheKey);

            if (!cachedOrders.IsNullOrEmpty)
            {
               
                return JsonConvert.DeserializeObject<List<OrderDTO>>(cachedOrders);
            }

            var ordersFromDb = await _orderRepository.GetAllAsync(userId, cancellationToken);
            var orderDTO = ConverOrderToOrderDTO(ordersFromDb);

            // Если данные есть, сериализуем и сохраняем в Redis
            if (orderDTO != null && orderDTO.Any())
            {
                
                await _redisDb.StringSetAsync(cacheKey,JsonConvert.SerializeObject(orderDTO),TimeSpan.FromMinutes(10)); 

            }
            return orderDTO ?? Enumerable.Empty<OrderDTO>();

        }

        public async Task<Model.Order> GetOrderByIdAsync(int userId, int entityId, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetByIdAsync(userId, entityId, cancellationToken);
        }

        public async Task DeleteOrderAsync(int entityId, CancellationToken cancellationToken)
        {
             await _orderRepository.DeleteAsync(entityId, cancellationToken);

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
                        OrderItemDTOs = order.OrderItems.Select(item => new OrderItemDTO{
                            OrderItemId = item.OrderItemId,
                            Price = item.Product.Price,
                            ProductImageUrl = item.Product.ImagePath,
                            ProductName = item.Product.Name,
                            Quantity = item.Quantity,   
                    
                        }).ToList(),
                    }).ToList();

            return orderDTO;
                    
        }

        Task<IEnumerable<Model.Order>> IOrderService.GetAllOrdersAsync(int userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
