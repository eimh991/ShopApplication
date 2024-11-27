using Microsoft.AspNetCore.Mvc;
using Shop.DTO;
using Shop.Interfaces;
using Shop.Service;

namespace Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDTO loginUserDTO)
        {
            /*
            var token = await ((UserService)_userService).Login(loginUserDTO.Email, loginUserDTO.Password);
            HttpContext.Response.Cookies.Append("test-cookie", token);
            return Ok();
            //return Ok( new { token = "Bearer " + token });
            */
            Console.WriteLine("Авторизация: " + loginUserDTO.Email);

            var token = await ((UserService)_userService).Login(loginUserDTO.Email, loginUserDTO.Password);

            HttpContext.Response.Cookies.Append("test-cookie", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None
            });
            return Ok(token);
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            var token = Request.Cookies["test-cookie"];
            Console.WriteLine("Полученный токен: " + token);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(); 
            }

            return Ok(); 
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("test-cookie");

            return Ok();
        }
    }
}
