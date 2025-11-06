using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/RestaurantController")]
    [ApiController]
    public class RestaurantController : Controller
    {
        private readonly IRestaurant _restaurant;

        public RestaurantController(IRestaurant restaurant)
        {
            _restaurant = restaurant;
        }

        /// <summary>
        /// Создание ресторана
        /// </summary>
        [HttpPost("CreateRestaurant")]
        public async Task<IActionResult> CreateRestaurant([FromForm] Restaurant restaurant)
        {
            await _restaurant.CreateRestaurant(restaurant);
            return Ok();
        }

        /// <summary>
        /// Получение всех ресторанов
        /// </summary>
        [HttpGet("GetAllRestaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurant.GetAllRestaurants();
            return Ok(restaurants);
        }

        /// <summary>
        /// Получение ресторана по ID
        /// </summary>
        [HttpGet("GetRestaurantById/{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _restaurant.GetRestaurantById(id);
            if (restaurant == null)
                return NotFound();
            return Ok(restaurant);
        }

        /// <summary>
        /// Обновление ресторана
        /// </summary>
        [HttpPut("UpdateRestaurant")]
        public async Task<IActionResult> UpdateRestaurant([FromForm] Restaurant restaurant)
        {
            await _restaurant.UpdateRestaurant(restaurant);
            return Ok();
        }

        /// <summary>
        /// Удаление ресторана
        /// </summary>
        [HttpDelete("DeleteRestaurant/{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            await _restaurant.DeleteRestaurant(id);
            return Ok();
        }

        /// <summary>
        /// Получение ресторанов по тематике
        /// </summary>
        [HttpGet("GetRestaurantsByTematic/{tematic}")]
        public async Task<IActionResult> GetRestaurantsByTematic(string tematic)
        {
            var restaurants = await _restaurant.GetRestaurantsByTematic(tematic);
            return Ok(restaurants);
        }
    }
}
