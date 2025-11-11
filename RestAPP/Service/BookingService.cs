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
        private readonly UserContext _Usercontext;

        public BookingService(BookingContext Bookingcontext, UserContext Usercontext)
        {
            _Bookingcontext = Bookingcontext;
            _Usercontext = Usercontext;
        }

        public async Task CreateBooking(Booking booking)
        {
            _Bookingcontext.Bookings.Add(booking);
            await _Bookingcontext.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingById(int userId, int restaurantId)
        {
            return await _Bookingcontext.Bookings.FindAsync(userId, restaurantId);
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return await _Bookingcontext.Bookings.ToListAsync();
        }
        public async Task UpdateBooking(Booking booking)
        {
            _Bookingcontext.Bookings.Update(booking);
            await _Bookingcontext.SaveChangesAsync();
        }

        public async Task DeleteBooking(int userId, int restaurantId)
        {
            var booking = await _Bookingcontext.Bookings
                .FirstOrDefaultAsync(b => b.User_Id == userId && b.Restaurant_Id == restaurantId);

            if (booking != null)
            {
                _Bookingcontext.Bookings.Remove(booking);
                await _Bookingcontext.SaveChangesAsync();
            }
        }

        public async Task<List<Booking>> GetBookingsByUserId(int userId)
        {
            return await _Bookingcontext.Bookings
                .Where(b => b.User_Id == userId)
                .ToListAsync();
        }
    }
}
