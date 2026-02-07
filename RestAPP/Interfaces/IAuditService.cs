using RestAPP.Model;

namespace RestAPP.Interfaces
{
    public interface IAuditService
    {
        Task LogAsync(int actorId, string actorType, string action, string entity, string? details = null);

        Task<List<ActivityLog>> GetAllLogsAsync();

        Task<List<ActivityLog>> GetLogsByActorAsync(int actorId);

        Task ClearAllAsync();

        Task TrimAsync(int keepLastCount = 1000);
    }
}