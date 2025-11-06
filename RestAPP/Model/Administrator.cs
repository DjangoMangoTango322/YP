using System.ComponentModel.DataAnnotations;

namespace RestAPI.Model
{
    public class Administrator
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
