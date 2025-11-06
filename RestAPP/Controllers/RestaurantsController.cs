using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> GetRestaurants()
        {
            var restaurants = await _restaurantService.GetRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(id);
            if (restaurant == null)
                return NotFound();
            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<ActionResult<Restaurant>> CreateRestaurant(Restaurant restaurant)
        {
            var createdRestaurant = await _restaurantService.CreateRestaurantAsync(restaurant);
            return CreatedAtAction(nameof(GetRestaurant), new { id = createdRestaurant.Id }, createdRestaurant);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Restaurant>> UpdateRestaurant(int id, Restaurant restaurant)
        {
            if (id != restaurant.Id)
                return BadRequest();

            var updatedRestaurant = await _restaurantService.UpdateRestaurantAsync(id, restaurant);
            if (updatedRestaurant == null)
                return NotFound();

            return Ok(updatedRestaurant);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRestaurant(int id)
        {
            var success = await _restaurantService.DeleteRestaurantAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}