using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Desktop.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } 
        public string SpecialRequests { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UserName { get; set; }
        public string RestaurantName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }

        public Visibility ShowConfirmButton =>
            Status == "pending" ? Visibility.Visible : Visibility.Collapsed;

        public Booking()
        {
            CreatedAt = DateTime.Now;
            Status = "pending";
        }
    }
}