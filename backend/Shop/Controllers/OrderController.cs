using Microsoft.AspNetCore.Mvc;
using Shop.Attribute;
using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public OrderController(IUserService userService, IOrderService orderService)
        {
            _orderService = orderService;
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var items = await ((UserService)_userService).GetUserCartItemsAsync(userId);
            if (items != null &&  items != Enumerable.Empty<CartItem>())
            {
                await _orderService.CreateOrderAsync(userId, items);
                return Ok();
            }
            return BadRequest("У вас нету товаров в корзине, вы не можете создать заказ");
        }
        [HttpGet("id")]
        public async Task<ActionResult<Order>>GetOrderAsync(int userId, int entityId)
        {
            var order = await _orderService.GetOrderByIdAsync(userId, entityId);
            if(order != null)
            {
                return Ok(order);
            }
            return NotFound(new { Messge = "По вашему запросу заказ не найден" });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrderAsync(int userId)
        {
            var orders = await _orderService.GetAllOrdersAsync(userId);
            if (orders != null)
            {
                return Ok(orders);
            }
            return NotFound(new { Messge = "У вас нету заказов" });
        }

        [HttpDelete("id")]
        [AuthorizeRole(UserRole.Admin)]
        public async Task<IActionResult> DeleteOrderAsync(int entityId)
        {
             await _orderService.DeleteOrderAsync(entityId);
            return Ok();
         
        }
    }
}
