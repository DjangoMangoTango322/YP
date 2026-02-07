using System.ComponentModel.DataAnnotations;

namespace RestAPP.Model
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        public int ActorId { get; set; }                // ID пользователя или администратора
        [Required, StringLength(50)]
        public string ActorType { get; set; }           // "User" или "Admin"

        [Required, StringLength(50)]
        public string Action { get; set; }              // LOGIN, REGISTER, CREATE, UPDATE, DELETE, BOOKING и т.д.

        [Required, StringLength(50)]
        public string Entity { get; set; }              // User, Booking, Restaurant, Dish, RestaurantDish, Auth и т.д.

        [StringLength(1000)]
        public string? Details { get; set; }            // дополнительная информация

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
