using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int RestaurantId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int PreparationTime { get; set; } 
        public string RestaurantName { get; set; }

        public Dish()
        {
            IsAvailable = true;
            PreparationTime = 15;
        }
    }
}