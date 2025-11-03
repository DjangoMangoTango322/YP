using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Models
{
    public class Booking : INotifyPropertyChanged
    {
        private int _id;
        private int _userId;
        private int _restaurantId;
        private DateTime _bookingDate;
        private TimeSpan _bookingTime;
        private int _numberOfGuests;
        private string _status;
        private DateTime _createdAt;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public int UserId
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(nameof(UserId)); }
        }

        public int RestaurantId
        {
            get => _restaurantId;
            set { _restaurantId = value; OnPropertyChanged(nameof(RestaurantId)); }
        }

        public DateTime BookingDate
        {
            get => _bookingDate;
            set { _bookingDate = value; OnPropertyChanged(nameof(BookingDate)); }
        }

        public TimeSpan BookingTime
        {
            get => _bookingTime;
            set { _bookingTime = value; OnPropertyChanged(nameof(BookingTime)); }
        }

        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set { _numberOfGuests = value; OnPropertyChanged(nameof(NumberOfGuests)); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(nameof(CreatedAt)); }
        }

        // Вычисляемые свойства для отображения
        public string ClientName => $"Клиент #{UserId}";
        public string RestaurantName => $"Ресторан #{RestaurantId}";

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

