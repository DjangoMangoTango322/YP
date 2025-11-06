using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        public RestaurantContext()
        {
            Database.EnsureCreated();
            Restaurants.Load();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConnection.config);
        }
    }
}
