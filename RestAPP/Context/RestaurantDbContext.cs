using Microsoft.EntityFrameworkCore;
using RestAPI.Model;

namespace RestAPI.Context
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RestaurantDish> RestaurantDishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship for Restaurant-Dish
            modelBuilder.Entity<RestaurantDish>()
                .HasKey(rd => new { rd.RestaurantId, rd.DishId });

            modelBuilder.Entity<RestaurantDish>()
                .HasOne(rd => rd.Restaurant)
                .WithMany()
                .HasForeignKey(rd => rd.RestaurantId);

            modelBuilder.Entity<RestaurantDish>()
                .HasOne(rd => rd.Dish)
                .WithMany()
                .HasForeignKey(rd => rd.DishId);

            // Configure relationships for Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Restaurant)
                .WithMany()
                .HasForeignKey(b => b.RestaurantId);
        }
    }
}