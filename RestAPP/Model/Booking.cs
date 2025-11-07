using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Model
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } // 🔧 Добавлено свойство навигации

        [Required]
        public int RestaurantId { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; } // 🔧 Можно добавить, если есть связь

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        public string Status { get; set; } = "Ожидание";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
