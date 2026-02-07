using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Achievement
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } // "Новичок" и т.д.
        public string Description { get; set; }
        public int Threshold { get; set; } // Количество бронирований для получения (1, 5, 10, 15)
    }
}