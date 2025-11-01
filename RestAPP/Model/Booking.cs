using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int RestaurantId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public TimeSpan BookingTime { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NumberOfGuests { get; set; }

        public string Status { get; set; } = "pending";

        public DateTime CreatedAt { get; set; }
    }
}
