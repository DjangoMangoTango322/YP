using System.ComponentModel.DataAnnotations;

namespace RestAPP.Model
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }

        public int ActorId { get; set; }                
        [Required, StringLength(50)]
        public string ActorType { get; set; }           

        [Required, StringLength(50)]
        public string Action { get; set; }              

        [Required, StringLength(50)]
        public string Entity { get; set; }              

        [StringLength(1000)]
        public string? Details { get; set; }          

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
