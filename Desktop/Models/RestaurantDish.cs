using System.ComponentModel;

namespace Desktop.Models
{
    public class RestaurantDish : INotifyPropertyChanged
    {
        private int _restaurantId;
        private int _dishId;

        public int RestaurantId
        {
            get => _restaurantId;
            set { _restaurantId = value; OnPropertyChanged(nameof(RestaurantId)); }
        }

        public int DishId
        {
            get => _dishId;
            set { _dishId = value; OnPropertyChanged(nameof(DishId)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}