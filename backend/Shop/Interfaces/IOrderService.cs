using Shop.Model;

namespace Shop.Interfaces
{
    public interface IOrderService
    {
        public Task CreateOrderAsync(int userId, IEnumerable<CartItem> items);
        public Task<Order> GetOrderByIdAsync(int userId, int entityId);

        public Task<IEnumerable<Order>> GetAllOrdersAsync(int userId);

        public Task DeleteOrderAsync(int entityId);
    }
}
