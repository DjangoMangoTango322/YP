using Microsoft.EntityFrameworkCore;
using RestAPP.Model;


namespace RestAPP.Context
{
    public class AuditContext
    {
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        public AuditContext(DbContextOptions<AuditContext> options)
            : base(options)
        {
            Database.EnsureCreated(); 
        }
    }
}
