using Microsoft.AspNetCore.Mvc;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/AdministratorController")]
    [ApiController]
    public class AdministratorController : Controller
    {
        private readonly IAdministrator _administrator;

        public AdministratorController(IAdministrator administrator)
        {
            _administrator = administrator;
        }

        /// <summary>
        /// Регистрация Администратора
        /// </summary>
        [Route("AddAdministrator")]
        [HttpPost]
        public Task AddAdministrator([FromForm] Administrator administrator)
        {
            var res = _administrator.ReginA(administrator);
            return res;
        }

        /// <summary>
        /// Авторизация Администратора
        /// </summary>
        [HttpPost("LoginAdministrator")]
        public async Task<IActionResult> LoginAdministrator([FromForm] string login, [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Логин и пароль обязательны");

            var result = await _administrator.LoginAdmin(login, password);

            if (result <= 0)
                return Unauthorized();

            return Ok(new { AdministratorId = result });
        }

        /// <summary>
        /// Получение всех администраторов
        /// </summary>
        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _administrator.GetAllAdmins();
            return Ok(admins);
        }

        /// <summary>
        /// Получение администратора по ID
        /// </summary>
        [HttpGet("GetAdminById/{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _administrator.GetAdminById(id);
            if (admin == null)
                return NotFound();
            return Ok(admin);
        }

        /// <summary>
        /// Обновление администратора
        /// </summary>
        [HttpPut("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromForm] Administrator admin)
        {
            await _administrator.UpdateAdmin(admin);
            return Ok();
        }

        /// <summary>
        /// Удаление администратора по ID
        /// </summary>
        [HttpDelete("DeleteAdmin/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            await _administrator.DeleteAdmin(id);
            return Ok();
        }
    }
}
