using RestAPI.Model;

namespace RestAPI.Interfaces
{
    public interface IBooking
    {
        Task CreateBooking(Booking booking);
        Task<Booking> GetBookingById(int id);
        Task<List<Booking>> GetAllBookings();
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(int id);
        Task<List<Booking>> GetBookingsByUserId(int userId);
    }
}
