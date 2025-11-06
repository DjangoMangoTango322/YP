using Microsoft.EntityFrameworkCore;
using RestAPI.Model;

namespace RestAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<RestaurantDish> RestaurantDishes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Administrator> Administrators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for RestaurantDish
            modelBuilder.Entity<RestaurantDish>()
                .HasKey(rd => new { rd.RestaurantId, rd.DishId });

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure Booking entity
            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasDefaultValue("pending");

            modelBuilder.Entity<Booking>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
