using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Models
{
    public class RestaurantDish
    {
        public int RestaurantId { get; set; }
        public int DishId { get; set; }
        public string RestaurantName { get; set; }
        public string DishName { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public RestaurantDish()
        {
            IsAvailable = true;
        }
    }
}