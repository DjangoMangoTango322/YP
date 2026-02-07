using System.ComponentModel.DataAnnotations;
using RestAPI.Model;

namespace RestAPI.Model
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievementId { get; set; }
        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства (для удобства выборки)
        public virtual User User { get; set; }
        public virtual Achievement Achievement { get; set; }
    }
}