using Microsoft.AspNetCore.Mvc;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/RestaurantDishController")]
    [ApiController]
    public class RestaurantDishController : Controller
    {
        private readonly IRestaurantDish _restaurantDish;

        public RestaurantDishController(IRestaurantDish restaurantDish)
        {
            _restaurantDish = restaurantDish;
        }

        /// <summary>
        /// Добавление блюда в ресторан
        /// </summary>
        [HttpPost("AddDishToRestaurant")]
        public async Task<IActionResult> AddDishToRestaurant([FromForm] RestaurantDish restaurantDish)
        {
            await _restaurantDish.AddDishToRestaurant(restaurantDish);
            return Ok();
        }

        /// <summary>
        /// Удаление блюда из ресторана
        /// </summary>
        [HttpDelete("RemoveDishFromRestaurant")]
        public async Task<IActionResult> RemoveDishFromRestaurant([FromForm] int restaurantId, [FromForm] int dishId)
        {
            await _restaurantDish.RemoveDishFromRestaurant(restaurantId, dishId);
            return Ok();
        }

        /// <summary>
        /// Получение блюд ресторана
        /// </summary>
        [HttpGet("GetDishesByRestaurantId/{restaurantId}")]
        public async Task<IActionResult> GetDishesByRestaurantId(int restaurantId)
        {
            var dishes = await _restaurantDish.GetDishesByRestaurantId(restaurantId);
            return Ok(dishes);
        }

        /// <summary>
        /// Получение ресторанов по блюду
        /// </summary>
        [HttpGet("GetRestaurantsByDishId/{dishId}")]
        public async Task<IActionResult> GetRestaurantsByDishId(int dishId)
        {
            var restaurants = await _restaurantDish.GetRestaurantsByDishId(dishId);
            return Ok(restaurants);
        }
    }
}
