using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
    }
}
