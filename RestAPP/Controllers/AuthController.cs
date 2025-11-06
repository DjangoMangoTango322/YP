using Microsoft.AspNetCore.Mvc;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
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

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginRequest loginRequest)
        {
            var user = await _userService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
                return Unauthorized("Неверный email или пароль");

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest registerRequest)
        {
            var user = new User
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Phone = registerRequest.Phone,
                Password = registerRequest.Password,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(Login), new { id = createdUser.Id }, createdUser);
        }
    }
}