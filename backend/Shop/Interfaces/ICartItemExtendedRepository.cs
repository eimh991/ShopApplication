using Shop.DTO;
using Shop.Model;

namespace Shop.Interfaces
{
    public interface ICartItemExtendedRepository
    {
        Task<int> AddAsyncAndReturnCartItemId(int userId, CartItem cartItem);
        Task UpdateQuantityAsync(CartItem cartItem);
        Task<IEnumerable<CartProductDTO>> GetAllCartProductAsync(int userId);
    }
}
