using RestAPI.Model;

namespace RestAPI.Interfaces
{
    public interface IBooking
    {
        Task CreateBooking(Booking booking);
        Task<Booking> GetBookingById(int userId, int restaurantId);
        Task<List<Booking>> GetAllBookings();
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(int userId, int restaurantId);
        Task<List<Booking>> GetBookingsByUserId(int userId);
    }
}
