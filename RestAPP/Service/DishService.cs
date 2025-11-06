using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;
using RestAPP.Context;

namespace RestAPI.Service
{
    public class DishService : IDish
    {
        private readonly DishContext _Dishcontext;

        public DishService(DishContext Dishcontext)
        {
            _Dishcontext = Dishcontext;
        }

        public async Task CreateDish(Dish dish)
        {
            _Dishcontext.Dishes.Add(dish);
            await _Dishcontext.SaveChangesAsync();
        }

        public async Task<Dish> GetDishById(int id)
        {
            return await _Dishcontext.Dishes.FindAsync(id);
        }

        public async Task<List<Dish>> GetAllDishes()
        {
            return await _Dishcontext.Dishes.ToListAsync();
        }

        public async Task UpdateDish(Dish dish)
        {
            _Dishcontext.Dishes.Update(dish);
            await _Dishcontext.SaveChangesAsync();
        }

        public async Task DeleteDish(int id)
        {
            var dish = await _Dishcontext.Dishes.FindAsync(id);
            if (dish != null)
            {
                _Dishcontext.Dishes.Remove(dish);
                await _Dishcontext.SaveChangesAsync();
            }
        }

        public async Task<List<Dish>> GetDishesByCategory(string category)
        {
            return await _Dishcontext.Dishes
                .Where(d => d.Category == category)
                .ToListAsync();
        }
    }
}