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
        private readonly AchievementContext _achievementContext;

        public BookingService(BookingContext Bookingcontext, UserContext Usercontext, AchievementContext achievementContext)
        {
            _Bookingcontext = Bookingcontext;
            _Usercontext = Usercontext;
            _achievementContext = achievementContext;
        }

        public async Task CreateBooking(Booking booking)
        {
            _Bookingcontext.Bookings.Add(booking);
            await _Bookingcontext.SaveChangesAsync();

            await CheckAndAwardAchievements(booking.User_Id);
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
            var booking = await _Bookingcontext.Bookings.FindAsync(userId, restaurantId);
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
        private async Task CheckAndAwardAchievements(int userId)
        {
            int count = await _Bookingcontext.Bookings.CountAsync(b => b.User_Id == userId);

            var allAchievements = await _achievementContext.Achievements.ToListAsync();

            foreach (var ach in allAchievements)
            {
                if (count >= ach.Threshold)
                {
                    bool hasIt = await _achievementContext.UserAchievements
                        .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == ach.Id);

                    if (!hasIt)
                    {
                        _achievementContext.UserAchievements.Add(new UserAchievement
                        {
                            UserId = userId,
                            AchievementId = ach.Id,
                            UnlockedAt = DateTime.UtcNow
                        });
                    }
                }
            }
            await _achievementContext.SaveChangesAsync();
        }
    }
}
