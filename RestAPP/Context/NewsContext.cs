using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Model;
using RestAPI.Models;

namespace RestAPP.Context
{
    public class NewsContext : DbContext
    {
        public DbSet<News> News { get; set; }

        public NewsContext(DbContextOptions<NewsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Эта строка гарантирует, что EF пойдет в таблицу "News" в схеме "dbo"
            modelBuilder.Entity<News>().ToTable("News", "dbo");
        }
    }
}