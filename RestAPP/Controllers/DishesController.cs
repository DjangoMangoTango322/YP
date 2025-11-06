using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishesController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Dish>>> GetDishes()
        {
            var dishes = await _dishService.GetDishesAsync();
            return Ok(dishes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var dish = await _dishService.GetDishAsync(id);
            if (dish == null)
                return NotFound();
            return Ok(dish);
        }

        [HttpPost]
        public async Task<ActionResult<Dish>> CreateDish(Dish dish)
        {
            var createdDish = await _dishService.CreateDishAsync(dish);
            return CreatedAtAction(nameof(GetDish), new { id = createdDish.Id }, createdDish);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Dish>> UpdateDish(int id, Dish dish)
        {
            if (id != dish.Id)
                return BadRequest();

            var updatedDish = await _dishService.UpdateDishAsync(id, dish);
            if (updatedDish == null)
                return NotFound();

            return Ok(updatedDish);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDish(int id)
        {
            var success = await _dishService.DeleteDishAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
