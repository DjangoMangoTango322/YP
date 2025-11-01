using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
