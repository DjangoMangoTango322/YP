using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public TimeSpan BookingTime { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        public string Status { get; set; } = "Ожидание";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
