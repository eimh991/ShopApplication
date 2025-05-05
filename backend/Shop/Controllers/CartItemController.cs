using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<int>> CreateCartItem(CartItemDTO cartItemDTO, CancellationToken cancellationToken)
        {
           var carItemId =  await _cartItemService.CreateCartItemAsync(cartItemDTO,cancellationToken);

            return Ok(carItemId);
        }

        [HttpGet("id")]
        public async Task<ActionResult<CartItem>> GetCartItem(int userId,  int cartItemId, CancellationToken cancellationToken)
        {
          var cartItem  =   await _cartItemService.GetCartItemByIdAsync(userId, cartItemId,cancellationToken);
            if (cartItem != null)
            {
                return Ok(cartItem);
            }
            return NotFound(new { Messge = "Данный продукт в вашей корзине не найден" });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardItemResponseDTO>>> GetAllCartItems(int userId, CancellationToken cancellationToken)
        {
            var items = await _cartItemService.GetAllCartItemsAsync(userId, cancellationToken);
            if (items == null || items == Enumerable.Empty<CardItemResponseDTO>())
            {
                return Ok(Enumerable.Empty<CardItemResponseDTO>());
            }
            return Ok(items);
        }

        [HttpPut("QuentityChange")]
        public async Task<IActionResult> QuentityChange(int cartItemId, int quantity, CancellationToken cancellationToken)
        {
            await _cartItemService.UpdateCountCartItemsAsync(cartItemId, quantity, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int cartItemId, CancellationToken cancellationToken)
        {
           await _cartItemService.DeleteCartItemAsync(cartItemId, cancellationToken);

            return Ok();
        }

        
        [HttpGet("CartProducts")]
        public async Task<IEnumerable<CartProductDTO>> GetAllCartProduct([FromQuery] int userId, CancellationToken cancellationToken)
        {
            var cartProducts = await ((CartItemSercive)_cartItemService).GetAllCartProductAsync(userId, cancellationToken);

            return cartProducts;
        }
    }
}
