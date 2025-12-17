using Microsoft.AspNetCore.Mvc;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/AdministratorController")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministrator _administrator;

        public AdministratorController(IAdministrator administrator)
        {
            _administrator = administrator;
        }

        [HttpPost("AddAdministrator")]
        public async Task<IActionResult> AddAdministrator([FromBody] Administrator administrator)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _administrator.ReginA(administrator);
            return Ok();
        }

        [HttpPost("LoginAdministrator")]
        public async Task<IActionResult> LoginAdministrator([FromForm] string login, [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return BadRequest("Логин и пароль обязательны");

            var result = await _administrator.LoginAdmin(login, password);

            if (result <= 0)
                return Unauthorized("Неверный логин или пароль");

            var admin = await _administrator.GetAdminById(result);
            return Ok(admin); // Возвращаем объект Administrator, как ожидает клиент
        }

        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _administrator.GetAllAdmins();
            return Ok(admins);
        }

        [HttpGet("GetAdminById/{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _administrator.GetAdminById(id);
            if (admin == null)
                return NotFound();
            return Ok(admin);
        }

        [HttpPost("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromBody] Administrator admin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _administrator.UpdateAdmin(admin);
            return Ok();
        }

        [HttpDelete("DeleteAdmin/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            await _administrator.DeleteAdmin(id);
            return Ok();
        }
    }
}