using Microsoft.EntityFrameworkCore;
using RestAPI.Model;
using RestAPP.Context;
using RestAPP.Interfaces;

namespace RestAPP.Service
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
                .FirstOrDefaultAsync(rd => rd.RestaurantId == restaurantId && rd.DishId == dishId);

            if (restaurantDish != null)
            {
                _RestaurantDishcontext.RestaurantDishes.Remove(restaurantDish);
                await _RestaurantDishcontext.SaveChangesAsync();
            }
        }

        public async Task<List<Dish>> GetDishesByRestaurantId(int restaurantId)
        {
            var dishIds = await _RestaurantDishcontext.RestaurantDishes
                .Where(rd => rd.RestaurantId == restaurantId)
                .Select(rd => rd.DishId)
                .ToListAsync();

            return await _Dishcontext.Dishes
                .Where(d => dishIds.Contains(d.Id))
                .ToListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurantsByDishId(int dishId)
        {
            var restaurantIds = await _RestaurantDishcontext.RestaurantDishes
                .Where(rd => rd.DishId == dishId)
                .Select(rd => rd.RestaurantId)
                .ToListAsync();

            return await _Restaurantcontext.Restaurants
                .Where(r => restaurantIds.Contains(r.Id))
                .ToListAsync();
        }
    }
}
