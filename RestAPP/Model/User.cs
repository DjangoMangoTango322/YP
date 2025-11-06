using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
        public string Login { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
