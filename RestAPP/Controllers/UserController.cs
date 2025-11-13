using Microsoft.AspNetCore.Mvc;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/UserController")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUser _user;

        public UserController(IUser user)
        {
            _user = user;
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        [Route("AddUser")]
        [HttpPost]
        public Task AddUser([FromForm] User user)
        {
            var res = _user.ReginU(user);
            return res;
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromForm] string login, [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Email и пароль обязательны");

            var result = await _user.LoginUser(login, password);

            if (result <= 0)
                return Unauthorized();

            return Ok(new { UserId = result });
        }

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _user.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// Получение пользователя по ID
        /// </summary>
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _user.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        
        /// <summary>
        /// Обновление пользователя
        /// </summary>
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] User user)
        {
            await _user.UpdateUser(user);
            return Ok();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id) 
        {
            await _user.DeleteUser(id);
            return Ok();
        }

        /// <summary>
        /// Проверка учетных данных
        /// </summary>
        [HttpPost("ValidateUserCredentials")]
        public async Task<IActionResult> ValidateUserCredentials([FromForm] string login, [FromForm] string password)
        {
            var isValid = await _user.ValidateUserCredentials(login, password);
            return Ok(new { IsValid = isValid });
        }
    }
}