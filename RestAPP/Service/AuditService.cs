using RestAPP.Context;
using RestAPP.Interfaces;
using RestAPP.Model;
using Microsoft.EntityFrameworkCore;

namespace RestAPP.Service
{
    public class AuditService : IAuditService
    {
        private readonly AuditContext _context;

        public AuditService(AuditContext context)
        {
            _context = context;
        }

        public async Task LogAsync(int actorId, string actorType, string action, string entity, string? details = null)
        {
            var log = new ActivityLog
            {
                ActorId = actorId,
                ActorType = actorType,
                Action = action,
                Entity = entity,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _context.ActivityLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ActivityLog>> GetAllLogsAsync()
        {
            return await _context.ActivityLogs
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task<List<ActivityLog>> GetLogsByActorAsync(int actorId)
        {
            return await _context.ActivityLogs
                .Where(l => l.ActorId == actorId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task ClearAllAsync()
        {
            _context.ActivityLogs.RemoveRange(_context.ActivityLogs);
            await _context.SaveChangesAsync();
        }

        public async Task TrimAsync(int keepLastCount = 1000)
        {
            var toDelete = await _context.ActivityLogs
                .OrderByDescending(l => l.Timestamp)
                .Skip(keepLastCount)
                .ToListAsync();

            if (toDelete.Any())
            {
                _context.ActivityLogs.RemoveRange(toDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}