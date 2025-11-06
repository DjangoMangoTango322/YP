using RestAPI.Model;

namespace RestAPI.Interfaces
{
    public interface IAdministrator
    {
        Task ReginA(Administrator admin);
        Task<int> LoginAdmin(string login, string password);
        Task<Administrator> GetAdminById(int id);
        Task<List<Administrator>> GetAllAdmins();
        Task UpdateAdmin(Administrator admin);
        Task DeleteAdmin(int id);
    }
}
