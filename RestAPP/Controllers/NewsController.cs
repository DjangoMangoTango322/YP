using Microsoft.AspNetCore.Mvc;
using RestAPI.Interfaces;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("GetAllNews")]
        public async Task<IActionResult> GetAllNews()
        {
            var news = await _newsService.GetAllNews();
            return Ok(news);
        }

        [HttpPost("RefreshNews")]
        public async Task<IActionResult> RefreshNews()
        {
            await _newsService.ParseNewsFromSource();
            return Ok("База обновлена");
        }

        [HttpDelete("DeleteNews/{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            // Правильный вызов:
            await _newsService.DeleteNews(id);
            return Ok("Запись удалена");
        }
    }
}