using System.ComponentModel.DataAnnotations;

namespace Desktop.Models
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
        public bool IsSelected { get; set; } = false;
    }

}
