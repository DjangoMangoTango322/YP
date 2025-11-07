using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Dish
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Category { get; set; }
    }
}
