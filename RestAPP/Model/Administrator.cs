using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Administrator
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
