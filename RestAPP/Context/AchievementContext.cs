using Microsoft.EntityFrameworkCore;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class AchievementContext : DbContext
    {
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<User> Users { get; set; } // Нужно для связей

        public AchievementContext(DbContextOptions<AchievementContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
