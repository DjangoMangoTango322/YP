using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class RestaurantContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }

        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
 