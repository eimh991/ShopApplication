using Shop.DTO;
using Shop.Model;

namespace Shop.Interfaces
{
    public interface ICartItemExtendedRepository
    {
        Task<int> AddAsyncAndReturnCartItemId(int userId, CartItem cartItem, CancellationToken cancellationToken);
        Task UpdateQuantityAsync(CartItem cartItem, CancellationToken cancellationToken);
        Task<IEnumerable<CartProductDTO>> GetAllCartProductAsync(int userId, CancellationToken cancellationToken);
    }
}
