using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Achievement
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Threshold { get; set; } 
    }
}