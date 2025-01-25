using Microsoft.AspNetCore.Mvc;
using Shop.DTO;
using Shop.Interfaces;
using Shop.Model;
using Shop.Repositories;
using Shop.Service;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCartItem(CartItemDTO cartItemDTO)
        {
            await _cartItemService.CreateCartItemAsync(cartItemDTO);

            return Ok();
        }

        [HttpGet("id")]
        public async Task<ActionResult<CartItem>> GetCartItem(int userId,  int cartItemId)
        {
          var cartItem  =   await _cartItemService.GetCartItemByIdAsync(userId, cartItemId);
            if (cartItem != null)
            {
                return Ok(cartItem);
            }
            return NotFound(new { Messge = "Данный продукт в вашей корзине не найден" });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardItemResponseDTO>>> GetAllCartItems(int userId)
        {
            var items = await _cartItemService.GetAllCartItemsAsync(userId);
            if (items == null || items == Enumerable.Empty<CardItemResponseDTO>())
            {
                return Ok(Enumerable.Empty<CardItemResponseDTO>());
            }
            return Ok(items);
        }

        [HttpPut("QuentityChange")]
        public async Task<IActionResult> QuentityChange(int cartItemId, int quentity)
        {
            await _cartItemService.UpdateCountCartItemsAsync(cartItemId, quentity);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int cartItemId)
        {
           await _cartItemService.DeleteCartItemAsync(cartItemId);

            return Ok();
        }

        [HttpGet("GetAllCartProduct")]
        public async Task<IEnumerable<CartProductDTO>> GetAllCartProduct(int userId)
        {
            var cartProduct = await ((CartItemSercive)_cartItemService).GetAllCartProductAsync(userId);

            return cartProduct;
        }
    }
}
