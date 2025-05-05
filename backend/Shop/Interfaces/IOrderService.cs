using Shop.Model;

namespace Shop.Interfaces
{
    public interface IOrderService
    {
        public Task CreateOrderAsync(int userId, IEnumerable<CartItem> items, CancellationToken cancellationToken);
        public Task<Order> GetOrderByIdAsync(int userId, int entityId, CancellationToken cancellationToken);

        public Task<IEnumerable<Order>> GetAllOrdersAsync(int userId , CancellationToken cancellationToken);

        public Task DeleteOrderAsync(int entityId, CancellationToken cancellationToken);
    }
}
