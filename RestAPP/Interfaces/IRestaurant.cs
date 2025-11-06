using RestAPI.Model;

namespace RestAPI.Interfaces
{
    public interface IRestaurant
    {
        Task CreateRestaurant(Restaurant restaurant);
        Task<Restaurant> GetRestaurantById(int id);
        Task<List<Restaurant>> GetAllRestaurants();
        Task UpdateRestaurant(Restaurant restaurant);
        Task DeleteRestaurant(int id);
        Task<List<Restaurant>> GetRestaurantsByTematic(string tematic);
    }
}