using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantDishesController : ControllerBase
    {
        private readonly RestaurantDbContext _context;

        public RestaurantDishesController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddDishToRestaurant(RestaurantDish restaurantDish)
        {
            _context.RestaurantDishes.Add(restaurantDish);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveDishFromRestaurant(int restaurantId, int dishId)
        {
            var restaurantDish = await _context.RestaurantDishes
                .FirstOrDefaultAsync(rd => rd.RestaurantId == restaurantId && rd.DishId == dishId);

            if (restaurantDish == null) return NotFound();

            _context.RestaurantDishes.Remove(restaurantDish);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
