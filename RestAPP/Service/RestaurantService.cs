using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;
using RestAPP.Context;

namespace RestAPI.Service
{
    public class RestaurantService : IRestaurant
    {
        private readonly RestaurantContext _Restaurantcontext;

        public RestaurantService(RestaurantContext Restaurantcontext)
        {
            _Restaurantcontext = Restaurantcontext;
        }

        public async Task CreateRestaurant(Restaurant restaurant)
        {
            _Restaurantcontext.Restaurants.Add(restaurant);
            await _Restaurantcontext.SaveChangesAsync();
        }

        public async Task<Restaurant> GetRestaurantById(int id)
        {
            return await _Restaurantcontext.Restaurants.FindAsync(id);
        }

        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            return await _Restaurantcontext.Restaurants.ToListAsync();
        }

        public async Task UpdateRestaurant(Restaurant restaurant)
        {
            _Restaurantcontext.Restaurants.Update(restaurant);
            await _Restaurantcontext.SaveChangesAsync();
        }

        public async Task DeleteRestaurant(int id)
        {
            var restaurant = await _Restaurantcontext.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _Restaurantcontext.Restaurants.Remove(restaurant);
                await _Restaurantcontext.SaveChangesAsync();
            }
        }

        public async Task<List<Restaurant>> GetRestaurantsByTematic(string tematic)
        {
            return await _Restaurantcontext.Restaurants
                .Where(r => r.Tematic == tematic)
                .ToListAsync();
        }
    }
}