using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPP.Context;
using RestAPP.Services;

namespace RestAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiKitchenController : ControllerBase
    {
        private readonly GigaChatService _gigaChatService;
        private readonly DishContext _context; // ЗАМЕНИЛИ RestaurantContext на DishContext

        // В конструкторе также меняем тип контекста
        public AiKitchenController(GigaChatService gigaChatService, DishContext context)
        {
            _gigaChatService = gigaChatService;
            _context = context;
        }

        // Получить список всех блюд
        [HttpGet("dishes")]
        public async Task<IActionResult> GetAllDishes()
        {
            // Теперь _context.Dishes будет доступен, так как мы используем DishContext
            var dishes = await _context.Dishes.ToListAsync();
            return Ok(dishes);
        }

        // Получить AI описание для конкретного блюда
        [HttpGet("describe/{dishName}")]
        public async Task<IActionResult> GetDescription(string dishName)
        {
            try
            {
                var description = await _gigaChatService.GetDishDescriptionAsync(dishName);
                return Ok(new { description = description });
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка AI: {ex.Message}");
            }
        }
    }
}