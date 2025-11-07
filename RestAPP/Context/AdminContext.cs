using RestAPI.Context;
using Microsoft.EntityFrameworkCore;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class AdminContext : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; }

        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}