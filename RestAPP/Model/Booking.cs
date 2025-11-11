using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Model
{
    public class Booking
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

 
        public int Restaurant_Id { get; set; }

        

        public DateTime Booking_Date { get; set; }


        public int Number_Of_Guests { get; set; }

        public string Status { get; set; } = "Ожидание";

        public DateTime Created_At { get; set; } = DateTime.UtcNow;


    }
}
