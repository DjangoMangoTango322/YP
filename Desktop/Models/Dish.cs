using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}