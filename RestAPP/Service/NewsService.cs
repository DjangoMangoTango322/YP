using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using RestAPI.Interfaces;
using RestAPI.Models;
using RestAPP.Context;
using System.Net;

namespace RestAPI.Service
{
    // 1. Весь код ДОЛЖЕН быть внутри класса
    public class NewsService : INewsService
    {
        private readonly NewsContext _context;

        // Конструктор класса
        public NewsService(NewsContext context)
        {
            _context = context;
        }

        public async Task<List<News>> GetAllNews()
        {
            await DeleteOldNews();
            return await _context.News.OrderBy(n => n.PublishDate).ToListAsync();
        }

        public async Task ParseNewsFromSource()
        {
            // Новый URL
            string url = "https://59.ru/text/tags/restoran/";

            // Актуальные классы из вашего файла
            string cardClass = "wrap_RL97A";
            string titleClass = "header_RL97A";
            string descClass = "subtitle_RL97A";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");

            try
            {
                var html = await client.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // 1. Ищем карточки новостей
                var nodes = doc.DocumentNode.SelectNodes($"//div[contains(@class, '{cardClass}')]");

                if (nodes == null) return;

                foreach (var node in nodes)
                {
                    try
                    {
                        var titleNode = node.SelectSingleNode($".//a[contains(@class, '{titleClass}')]")
                                     ?? node.SelectSingleNode(".//h2")
                                     ?? node.SelectSingleNode(".//h3");

                        var descNode = node.SelectSingleNode($".//*[contains(@class, '{descClass}')]")
                                    ?? node.SelectSingleNode(".//p");

                        var imgNode = node.SelectSingleNode(".//img");

                        string title = WebUtility.HtmlDecode(titleNode?.InnerText?.Trim());
                        string desc = WebUtility.HtmlDecode(descNode?.InnerText?.Trim());
                        string img = imgNode?.GetAttributeValue("src", "");

                        if (!string.IsNullOrEmpty(title))
                        {
                            if (!await _context.News.AnyAsync(n => n.Title == title))
                            {
                                _context.News.Add(new News
                                {
                                    Title = title,
                                    Description = desc ?? "Новость из рубрики Рестораны",
                                    ImageUrl = img,
                                    PublishDate = DateTime.Now
                                });
                            }
                        }
                    }
                    catch { continue; }
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге 59.ru: {ex.Message}");
            }
        }
        public async Task DeleteNews(int id)
        {
            var item = await _context.News.FindAsync(id);
            if (item != null)
            {
                _context.News.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOldNews()
        {
            var limit = DateTime.Now.AddDays(-7);
            var oldItems = _context.News.Where(n => n.PublishDate < limit);
            if (await oldItems.AnyAsync())
            {
                _context.News.RemoveRange(oldItems);
                await _context.SaveChangesAsync();
            }
        }
    }
}