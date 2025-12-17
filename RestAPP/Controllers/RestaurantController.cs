using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/RestaurantController")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurant _restaurant;

        public RestaurantController(IRestaurant restaurant)
        {
            _restaurant = restaurant;
        }

        [HttpPost("CreateRestaurant")]
        public async Task<IActionResult> CreateRestaurant([FromBody] Restaurant restaurant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _restaurant.CreateRestaurant(restaurant);
            return Ok();
        }

        [HttpGet("GetAllRestaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurant.GetAllRestaurants();
            return Ok(restaurants);
        }

        [HttpGet("GetRestaurantById/{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _restaurant.GetRestaurantById(id);
            if (restaurant == null)
                return NotFound();
            return Ok(restaurant);
        }

        [HttpPost("UpdateRestaurant")]
        public async Task<IActionResult> UpdateRestaurant([FromBody] Restaurant restaurant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _restaurant.UpdateRestaurant(restaurant);
            return Ok();
        }

        [HttpDelete("DeleteRestaurant/{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            await _restaurant.DeleteRestaurant(id);
            return Ok();
        }
    }
}