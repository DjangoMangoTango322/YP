using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class RestaurantDishContext : DbContext
    {
        public DbSet<RestaurantDish> RestaurantDishes { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        public RestaurantDishContext(DbContextOptions<RestaurantDishContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RestaurantDish>()
                .HasKey(rd => new { rd.Restaurant_Id, rd.Dish_Id });
        }
    }
}