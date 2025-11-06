using Microsoft.AspNetCore.Mvc;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Регистрация Администратора
        /// </summary>
        /// <remarks>Данный метод для добавления администратора</remarks>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAdmin([FromForm] Administrator admin)
        {
            try
            {
                await _adminService.RegisterAdminAsync(admin);
                return Ok(new { Message = "Администратор успешно зарегистрирован", AdminId = admin.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Ошибка регистрации: {ex.Message}" });
            }
        }

        /// <summary>
        /// Авторизация Администратора
        /// </summary>
        /// <remarks>Данный метод авторизирует администратора в системе</remarks>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAdmin([FromForm] AdminLoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Login) || string.IsNullOrWhiteSpace(loginRequest.Password))
                return BadRequest("Логин и пароль обязательны");

            try
            {
                var adminId = await _adminService.LoginAdminAsync(loginRequest.Login, loginRequest.Password);

                if (adminId <= 0)
                    return Unauthorized("Неверный логин или пароль");

                return Ok(new { AdminId = adminId, Message = "Успешная авторизация" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Ошибка авторизации: {ex.Message}" });
            }
        }

        /// <summary>
        /// Получение информации об администраторе
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdmin(int id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            if (admin == null)
                return NotFound("Администратор не найден");

            // Не возвращаем пароль в ответе
            return Ok(new
            {
                admin.Id,
                admin.Login,
                admin.Nickname,
                admin.CreatedAt
            });
        }
    }
}