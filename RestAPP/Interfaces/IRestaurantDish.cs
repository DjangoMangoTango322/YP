using RestAPI.Model;

namespace RestAPI.Interfaces
{
    public interface IRestaurantDish
    {
        Task AddDishToRestaurant(RestaurantDish restaurantDish);
        Task RemoveDishFromRestaurant(int RestaurantId, int DishId);
        Task<List<Dish>> GetDishesByRestaurantId(int restaurantId);
        Task<List<Restaurant>> GetRestaurantsByDishId(int dishId);
    }
}
