using Microsoft.AspNetCore.Mvc;
using RestAPP.Interfaces;

namespace RestAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _auditService.GetAllLogsAsync();
            return Ok(logs);
        }

        [HttpGet("actor/{actorId}")]
        public async Task<IActionResult> GetByActor(int actorId)
        {
            var logs = await _auditService.GetLogsByActorAsync(actorId);
            return Ok(logs);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _auditService.ClearAllAsync();
            return Ok(new { message = "Журнал аудита полностью очищен" });
        }

        [HttpDelete("trim/{count}")]
        public async Task<IActionResult> Trim(int count = 1000)
        {
            await _auditService.TrimAsync(count);
            return Ok(new { message = $"Оставлено последних {count} записей" });
        }
    }
}
