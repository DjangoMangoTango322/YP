using Microsoft.EntityFrameworkCore;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class AdminContext : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; }

        public AdminContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ПРАВИЛЬНЫЙ ВАРИАНТ - без Trusted_Connection=True
                optionsBuilder.UseSqlServer("Server=10.0.201.112;Database=base1_ISP_22_4_16;User Id=ISP_22_4_16;Password=6GdNkdTL69su_;TrustServerCertificate=True;");
            }
        }
    }
}
