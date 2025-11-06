using RestAPI.Model;

namespace RestAPI.Interfaces
{
    public interface IUser
    {
        Task ReginU(User users);
        
        Task<User> GetUserById(int id);
        Task<List<User>> GetAllUsers();
        Task UpdateUser(User user);
        Task DeleteUser(int id);
        Task<bool> ValidateUserCredentials(string login, string password);
        Task<int> LoginUser(string login, string password);
    }
}