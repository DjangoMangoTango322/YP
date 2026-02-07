using RestAPI.Models;

namespace RestAPI.Interfaces
{
    public interface INewsService
    {
        Task<List<News>> GetAllNews();
        Task ParseNewsFromSource();

        Task DeleteNews(int id);

        Task DeleteOldNews();
    }
}