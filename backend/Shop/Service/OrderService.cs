using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;

namespace Shop.Service
{
    public class OrderService : IOrderService
    {

        private readonly IRepositoryWithUser<Order> _orderRepository;
        private readonly IRepositoryWithUser<CartItem> _cartItemRepository;

        public OrderService(IRepositoryWithUser<Order> orderRepository,IRepositoryWithUser<CartItem> cartItemRepository)
        {
            _orderRepository = orderRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task CreateOrderAsync(int userId,IEnumerable<CartItem> items)
        {
            var orderItems = ConvertCartItemsToOrderItems(items);
            var order = new Order
            {
                OrderItems = orderItems.ToList(),
                OrderDate = DateTime.Now,
                TotalAmount = AllOrderPrice(orderItems.ToList())
            };
            await _orderRepository.AddAsync(userId, order);
            await ((CartItemRepository)_cartItemRepository).DeleteAllCartItemsAsync(userId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(int userId)
        {
           return  await _orderRepository.GetAllAsync(userId)?? 
                Enumerable.Empty<Order>(); ;
     
        }

        public async Task<Order> GetOrderByIdAsync(int userId, int entityId)
        {
            return await _orderRepository.GetByIdAsync(userId, entityId);
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
