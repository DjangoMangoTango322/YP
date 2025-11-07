using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class DishContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }

        public DishContext(DbContextOptions<DishContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
