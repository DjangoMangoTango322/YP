using System;

namespace Desktop.Models
{
    public class UserAchievementDTO
    {
        public string UserName { get; set; }
        public string AchievementName { get; set; }
        public DateTime UnlockedAt { get; set; }
    }
}