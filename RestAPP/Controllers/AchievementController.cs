using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Model;
using RestAPP.Context;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly AchievementContext _context;

        public AchievementController(AchievementContext context)
        {
            _context = context;
        }

        // Инициализация базы данных начальными ачивками (вызвать один раз через Swagger)
        [HttpPost("InitAchievements")]
        public async Task<IActionResult> Init()
        {
            if (!await _context.Achievements.AnyAsync())
            {
                _context.Achievements.AddRange(
                    new Achievement { Name = "Новичок", Description = "Сделайте 1 заказ", Threshold = 1 },
                    new Achievement { Name = "Гурман", Description = "Сделайте 5 заказов", Threshold = 5 },
                    new Achievement { Name = "Любитель пообедать", Description = "Сделайте 10 заказов", Threshold = 10 },
                    new Achievement { Name = "Обжора", Description = "Сделайте 15 заказов", Threshold = 15 }
                );
                await _context.SaveChangesAsync();
                return Ok("Ачивки созданы");
            }
            return Ok("Ачивки уже есть");
        }

        // Для Android: получить ачивки конкретного пользователя
        [HttpGet("GetUserAchievements/{userId}")]
        public async Task<IActionResult> GetUserAchievements(int userId)
        {
            var list = await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .Select(ua => new
                {
                    ua.Achievement.Name,
                    ua.Achievement.Description,
                    ua.UnlockedAt
                })
                .ToListAsync();
            return Ok(list);
        }

        // Для Desktop: получить все выданные ачивки (кто и что получил)
        [HttpGet("GetAllUserAchievements")]
        public async Task<IActionResult> GetAllUserAchievements()
        {
            var list = await _context.UserAchievements
                .Include(ua => ua.User)
                .Include(ua => ua.Achievement)
                .Select(ua => new
                {
                    UserName = ua.User.First_Name + " " + ua.User.Last_Name,
                    AchievementName = ua.Achievement.Name,
                    UnlockedAt = ua.UnlockedAt
                })
                .ToListAsync();
            return Ok(list);
        }
    }
}