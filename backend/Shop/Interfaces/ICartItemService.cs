using Shop.DTO;
using Shop.Model;
using System.Collections.Generic;

namespace Shop.Interfaces
{
    public interface ICartItemService
    {
        Task<int> CreateCartItemAsync(CartItemDTO cartItemDTO, CancellationToken cancellationToken);
        Task DeleteCartItemAsync(int cartItemId, CancellationToken cancellationToken);
        Task<IEnumerable<CardItemResponseDTO>> GetAllCartItemsAsync(int userId, CancellationToken cancellationToken);
        Task<CartItem> GetCartItemByIdAsync(int userId, int entityId, CancellationToken cancellationToken);
        Task UpdateCountCartItemsAsync(int cartItemId, int quentity, CancellationToken cancellationToken);

        Task ClearAllCartItemsAsync(int userId, CancellationToken cancellationToken);
    }
}
