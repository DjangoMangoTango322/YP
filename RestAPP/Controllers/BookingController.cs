using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Controllers
{
    [Route("api/BookingController")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBooking _booking;

        public BookingController(IBooking booking)
        {
            _booking = booking;
        }

        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _booking.CreateBooking(booking);
            return Ok();
        }

        [HttpGet("GetAllBookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _booking.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("GetBookingById/{userId}/{restaurantId}")]
        public async Task<IActionResult> GetBookingById(int userId, int restaurantId)
        {
            var booking = await _booking.GetBookingById(userId, restaurantId);
            if (booking == null)
                return NotFound();
            return Ok(booking);
        }

        [HttpPost("UpdateBooking")]
        public async Task<IActionResult> UpdateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _booking.UpdateBooking(booking);
            return Ok();
        }

        [HttpDelete("DeleteBooking/{userId}/{restaurantId}")]
        public async Task<IActionResult> DeleteBooking(int userId, int restaurantId)
        {
            await _booking.DeleteBooking(userId, restaurantId);
            return Ok();
        }
    }
}
