using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;
using RestAPP.Context;

namespace RestAPI.Service
{
    public class BookingService : IBooking
    {
        private readonly BookingContext _Bookingcontext;

        public BookingService(BookingContext Bookingcontext)
        {
            _Bookingcontext = Bookingcontext;
        }

        public async Task CreateBooking(Booking booking)
        {
            _Bookingcontext.Bookings.Add(booking);
            await _Bookingcontext.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingById(int id)
        {
            return await _Bookingcontext.Bookings
                .Include(b => b.User)
                .Include(b => b.Restaurant)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return await _Bookingcontext.Bookings
                .Include(b => b.User)
                .Include(b => b.Restaurant)
                .ToListAsync();
        }

        public async Task UpdateBooking(Booking booking)
        {
            _Bookingcontext.Bookings.Update(booking);
            await _Bookingcontext.SaveChangesAsync();
        }

        public async Task DeleteBooking(int id)
        {
            var booking = await _Bookingcontext.Bookings.FindAsync(id);
            if (booking != null)
            {
                _Bookingcontext.Bookings.Remove(booking);
                await _Bookingcontext.SaveChangesAsync();
            }
        }

        public async Task<List<Booking>> GetBookingsByUserId(int userId)
        {
            return await _Bookingcontext.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Restaurant)
                .ToListAsync();
        }
    }
}
