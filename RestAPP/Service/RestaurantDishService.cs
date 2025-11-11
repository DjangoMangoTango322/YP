using Microsoft.EntityFrameworkCore;
using RestAPI.Interfaces;
using RestAPI.Model;
using RestAPP.Context;

namespace RestAPI.Service
{
    public class RestaurantDishService : IRestaurantDish
    {
        private readonly RestaurantDishContext _RestaurantDishcontext;
        private readonly DishContext _Dishcontext;
        private readonly RestaurantContext _Restaurantcontext;

        public RestaurantDishService(RestaurantDishContext RestaurantDishcontext, DishContext Dishcontext, RestaurantContext Restaurantcontext)
        {
            _RestaurantDishcontext = RestaurantDishcontext;
            _Dishcontext = Dishcontext;
            _Restaurantcontext = Restaurantcontext;
        }

        public async Task AddDishToRestaurant(RestaurantDish restaurantDish)
        {
            _RestaurantDishcontext.RestaurantDishes.Add(restaurantDish);
            await _RestaurantDishcontext.SaveChangesAsync();
        }

        public async Task RemoveDishFromRestaurant(int restaurantId, int dishId)
        {
            var restaurantDish = await _RestaurantDishcontext.RestaurantDishes
                .FirstOrDefaultAsync(rd => rd.Restaurant_Id == restaurantId && rd.Dish_Id == dishId);

            if (restaurantDish != null)
            {
                _RestaurantDishcontext.RestaurantDishes.Remove(restaurantDish);
                await _RestaurantDishcontext.SaveChangesAsync();
            }
        }

        public async Task<List<Dish>> GetDishesByRestaurantId(int restaurantId)
        {
            var dishIds = await _RestaurantDishcontext.RestaurantDishes
                .Where(rd => rd.Restaurant_Id == restaurantId)
                .Select(rd => rd.Dish_Id)
                .ToListAsync();

            return await _Dishcontext.Dishes
                .Where(d => dishIds.Contains(d.Id))
                .ToListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurantsByDishId(int dishId)
        {
            var restaurantIds = await _RestaurantDishcontext.RestaurantDishes
                .Where(rd => rd.Dish_Id == dishId)
                .Select(rd => rd.Restaurant_Id)
                .ToListAsync();

            return await _Restaurantcontext.Restaurants
                .Where(r => restaurantIds.Contains(r.Id))
                .ToListAsync();
        }
    }
}
