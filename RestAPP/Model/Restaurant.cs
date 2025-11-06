using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public TimeSpan OpenTime { get; set; }

        [Required]
        public TimeSpan CloseTime { get; set; }

        [Required]
        public string Tematic { get; set; }
    }
}
