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
        private readonly DishContext _context; 
        public AiKitchenController(GigaChatService gigaChatService, DishContext context)
        {
            _gigaChatService = gigaChatService;
            _context = context;
        }

        
        [HttpGet("dishes")]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _context.Dishes.ToListAsync();
            return Ok(dishes);
        }

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