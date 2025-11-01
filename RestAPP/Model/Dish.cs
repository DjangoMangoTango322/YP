using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Dish
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(50)]
        public string Category { get; set; }
    }
}
