using Microsoft.AspNetCore.Mvc;
using Shop.Attribute;
using Shop.DTO;
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
        public async Task<IActionResult> CreateOrder([FromQuery] int userId, CancellationToken cancellationToken)
        {
            var items = await ((UserService)_userService).GetUserCartItemsAsync(userId, cancellationToken);
            if (items != null &&  items != Enumerable.Empty<CartItem>())
            {
                await _orderService.CreateOrderAsync(userId, items,cancellationToken);
                return Ok();
            }
            return BadRequest("У вас нету товаров в корзине, вы не можете создать заказ");
        }
        [HttpGet("id")]
        public async Task<ActionResult<Order>>GetOrderAsync(int userId, int entityId, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(userId, entityId, cancellationToken);
            if(order != null)
            {
                return Ok(order);
            }
            return NotFound(new { Messge = "По вашему запросу заказ не найден" });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrderAsync(int userId, CancellationToken cancellationToken)
        {
            var ordersDTO = await ((OrderService)_orderService).GetAllOrdersDTOAsync(userId, cancellationToken);
            if (ordersDTO != null)
            {
                return Ok(ordersDTO);
            }
            return NotFound(new { Messge = "У вас нету заказов" });
        }

        [HttpDelete("id")]
        [AuthorizeRole(UserRole.Admin)]
        public async Task<IActionResult> DeleteOrderAsync(int entityId, CancellationToken cancellationToken)
        {
             await _orderService.DeleteOrderAsync(entityId, cancellationToken);
            return Ok();
         
        }
    }
}
