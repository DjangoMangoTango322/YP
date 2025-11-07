using Microsoft.EntityFrameworkCore;
using RestAPI.Model;

namespace RestAPI.Context
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}