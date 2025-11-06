using RestAPI.Model;

namespace RestAPI.Interfaces
{
    public interface IDish
    {
        Task CreateDish(Dish dish);
        Task<Dish> GetDishById(int id);
        Task<List<Dish>> GetAllDishes();
        Task UpdateDish(Dish dish);
        Task DeleteDish(int id);
        Task<List<Dish>> GetDishesByCategory(string category);
    }
}