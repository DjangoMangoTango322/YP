namespace RestAPI.Model
{
    public class RestaurantDish
    {
        public int RestaurantId { get; set; }
        public int DishId { get; set; }

        // Navigation properties
        public Restaurant Restaurant { get; set; }
        public Dish Dish { get; set; }
    }
}
