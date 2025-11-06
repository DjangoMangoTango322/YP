using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/DishController")]
    [ApiController]
    public class DishController : Controller
    {
        private readonly IDish _dish;

        public DishController(IDish dish)
        {
            _dish = dish;
        }

        /// <summary>
        /// Создание блюда
        /// </summary>
        [HttpPost("CreateDish")]
        public async Task<IActionResult> CreateDish([FromForm] Dish dish)
        {
            await _dish.CreateDish(dish);
            return Ok();
        }

        /// <summary>
        /// Получение всех блюд
        /// </summary>
        [HttpGet("GetAllDishes")]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _dish.GetAllDishes();
            return Ok(dishes);
        }

        /// <summary>
        /// Получение блюда по ID
        /// </summary>
        [HttpGet("GetDishById/{id}")]
        public async Task<IActionResult> GetDishById(int id)
        {
            var dish = await _dish.GetDishById(id);
            if (dish == null)
                return NotFound();
            return Ok(dish);
        }

        /// <summary>
        /// Обновление блюда
        /// </summary>
        [HttpPut("UpdateDish")]
        public async Task<IActionResult> UpdateDish([FromForm] Dish dish)
        {
            await _dish.UpdateDish(dish);
            return Ok();
        }

        /// <summary>
        /// Удаление блюда
        /// </summary>
        [HttpDelete("DeleteDish/{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            await _dish.DeleteDish(id);
            return Ok();
        }

        /// <summary>
        /// Получение блюд по категории
        /// </summary>
        [HttpGet("GetDishesByCategory/{category}")]
        public async Task<IActionResult> GetDishesByCategory(string category)
        {
            var dishes = await _dish.GetDishesByCategory(category);
            return Ok(dishes);
        }
    }
}