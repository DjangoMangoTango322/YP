using RestAPI.Model;

namespace RestAPP.Interfaces
{
    public interface IRestaurantDish
    {
        Task AddDishToRestaurant(RestaurantDish restaurantDish);
        Task RemoveDishFromRestaurant(int RestaurantId, int DishId);
        Task<List<Dish>> GetDishesByRestaurantId(int restaurantId);
        Task<List<Restaurant>> GetRestaurantsByDishId(int dishId);
    }
}
