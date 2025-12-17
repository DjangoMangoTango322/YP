using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/DishController")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDish _dish;

        public DishController(IDish dish)
        {
            _dish = dish;
        }

        [HttpPost("CreateDish")]
        public async Task<IActionResult> CreateDish([FromBody] Dish dish)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _dish.CreateDish(dish);
            return Ok();
        }

        [HttpGet("GetAllDishes")]
        public async Task<IActionResult> GetAllDishes()
        {
            var dishes = await _dish.GetAllDishes();
            return Ok(dishes);
        }

        [HttpGet("GetDishById/{id}")]
        public async Task<IActionResult> GetDishById(int id)
        {
            var dish = await _dish.GetDishById(id);
            if (dish == null)
                return NotFound();
            return Ok(dish);
        }

        [HttpPost("UpdateDish")]
        public async Task<IActionResult> UpdateDish([FromBody] Dish dish)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _dish.UpdateDish(dish);
            return Ok();
        }

        [HttpDelete("DeleteDish/{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            await _dish.DeleteDish(id);
            return Ok();
        }
    }
}