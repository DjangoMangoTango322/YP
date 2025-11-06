using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/BookingController")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBooking _booking;

        public BookingController(IBooking booking)
        {
            _booking = booking;
        }

        /// <summary>
        /// Создание бронирования
        /// </summary>
        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromForm] Booking booking)
        {
            await _booking.CreateBooking(booking);
            return Ok();
        }

        /// <summary>
        /// Получение всех бронирований
        /// </summary>
        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _booking.GetAllBookings();
            return Ok(bookings);
        }

        /// <summary>
        /// Получение бронирования по ID
        /// </summary>
        [HttpGet("GetBookingById/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _booking.GetBookingById(id);
            if (booking == null)
                return NotFound();
            return Ok(booking);
        }

        /// <summary>
        /// Обновление бронирования
        /// </summary>
        [HttpPut("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromForm] Booking booking)
        {
            await _booking.UpdateBooking(booking);
            return Ok();
        }

        /// <summary>
        /// Удаление бронирования
        /// </summary>
        [HttpDelete("DeleteBooking/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            await _booking.DeleteBooking(id);
            return Ok();
        }

        /// <summary>
        /// Получение бронирований пользователя
        /// </summary>
        [HttpGet("GetBookingsByUserId/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            var bookings = await _booking.GetBookingsByUserId(userId);
            return Ok(bookings);
        }
    }
}
