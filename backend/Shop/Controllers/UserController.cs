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
        public async Task<IActionResult> Register(UserDTO userDTO, CancellationToken cancellationToken)
        {
            await _userService.CreateAsync(userDTO, cancellationToken);

            return Ok();
        }

        //[Authorize]
        [HttpGet("getme")]
        public ActionResult<User> GetingUser()
        {

            /*var userStringId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;*/
            var token = getToken();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token not found in cookies.");
            }

            // Парсим токен
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Извлекаем значение из claim с типом Sid
            var userStringId = GetIdFromClaims(jwtToken);
            var userRole = GetRoleFromClaims(jwtToken);

            if (userStringId == null)
            {
                throw new Exception("Ошибка авторизационных данных");
            }
            int userId = Convert.ToInt32(userStringId);
            //var user = await _userService.GetByIdAsync(userId);

            return Ok(new {id=userId, role = userRole});
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(userId, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserDTO user, CancellationToken cancellationToken)
        {
            await _userService.UpdateAsync(user, cancellationToken);
            return Ok();
        }

        [HttpPost("status")]
        public async Task<IActionResult> ChangeStatusAsync(UserDTO userDTO, string status, CancellationToken cancellationToken)
        {
            await _userService.ChangeStatusAsync(userDTO.UserId,status, cancellationToken);

            return Ok();
        }

        [HttpGet("id")]
        [AuthorizeRole(UserRole.Admin)]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                throw new Exception("Нету такого пользователя");
            }
            return Ok(user);
        }

        [HttpGet("checkmoney")]
        public async Task<ActionResult<bool>> CheckMoneyInAccount(int userId, decimal cartCoast, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new Exception("Нету такого пользователя");
            }
            if (user.Balance >= cartCoast)
            {
                return true;
            }
            return false;
        }

        private string GetIdFromClaims(JwtSecurityToken jwt)
        {
            return jwt?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        }

        private string GetRoleFromClaims(JwtSecurityToken jwt)
        {
            return jwt?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        }

        private string getToken()
        {
            return Request.Cookies["test-cookie"];
        } 

    }
}
