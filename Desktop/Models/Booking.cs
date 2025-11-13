using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Restaurant_Id { get; set; }
        public DateTime Booking_Date { get; set; }
        public int Number_Of_Guests { get; set; }
        public string Status { get; set; } = "Ожидание";
        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public bool IsSelected { get; set; } = false;
    }
}