using Shop.DTO;
using Shop.Model;
using System.Collections.Generic;

namespace Shop.Interfaces
{
    public interface ICartItemService
    {
        Task CreateCartItemAsync(CartItemDTO cartItemDTO);
        Task DeleteCartItemAsync(int cartItemId);
        Task<IEnumerable<CardItemResponseDTO>> GetAllCartItemsAsync(int userId);
        Task<CartItem> GetCartItemByIdAsync(int userId, int entityId);
        Task UpdateCountCartItemsAsync(int userId, int cartItemId, int quentity);

        Task ClearAllCartItemsAsync(int userId);
    }
}
