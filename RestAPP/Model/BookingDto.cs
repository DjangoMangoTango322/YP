namespace RestAPI.Model
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CreateBookingDto
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
    }

    public class UpdateBookingStatusDto
    {
        public string Status { get; set; }
    }
}
