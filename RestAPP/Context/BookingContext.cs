using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Model;

namespace RestAPP.Context
{
    public class BookingContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }

        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
