using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Attribute;
using Shop.DTO;
using Shop.Enum;
using Shop.Interfaces;
using Shop.Model;
using Shop.Service;
using Shop.UsersDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            await _userService.CreateAsync(userDTO);

            return Ok();
        }

        //[Authorize]
        [HttpGet("getme")]
        public async Task<ActionResult<User>> GetingUser()
        {

            /*var userStringId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;*/
            var token = Request.Cookies["test-cookie"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token not found in cookies.");
            }

            // Парсим токен
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Извлекаем значение из claim с типом Sid
            var userStringId = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var userRole = jwtToken?.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Role)?.Value;

            if (userStringId == null)
            {
                throw new Exception("Ошибка авторизационных данных");
            }
            int userId = Convert.ToInt32(userStringId);
            //var user = await _userService.GetByIdAsync(userId);

            return Ok(new {id=userId, role = userRole});
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _userService.DeleteAsync(userId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDTO user)
        {
            await _userService.UpdateAsync(user);
            return Ok();
        }

        [HttpPost("status")]
        public async Task<IActionResult> ChangeStatusAsync(UserDTO userDTO, string status)
        {
            await _userService.ChangeStatusAsync(userDTO.UserId,status);

            return Ok();
        }

        [HttpGet("id")]
        [AuthorizeRole(UserRole.Admin)]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Нету такого пользователя");
            }
            return Ok(user);
        }

    }
}
