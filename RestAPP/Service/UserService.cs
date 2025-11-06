using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;

namespace RestAPI.Service
{
    public class UserService : IUser
    {
        private readonly UserContext _Usercontext;

        public UserService(UserContext Usercontext)
        {
            _Usercontext = Usercontext;
        }

        public async Task ReginU(User users)
        {
            _Usercontext.Users.Add(users);
            await _Usercontext.SaveChangesAsync();

        }

        public async Task<int> LoginUser(string login, string password)
        {
            User User = _Usercontext.Users.Where(x => x.Login == login && x.Password == password).First();
            return User.Id;
        }


        public async Task<User> GetUserById(int id)
        {
            return await _Usercontext.Users.FindAsync(id);
        }

        

        public async Task<List<User>> GetAllUsers()
        {
            return await _Usercontext.Users.ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            _Usercontext.Users.Update(user);
            await _Usercontext.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _Usercontext.Users.FindAsync(id);
            if (user != null)
            {
                _Usercontext.Users.Remove(user);
                await _Usercontext.SaveChangesAsync();
            }
        }

        public async Task<bool> ValidateUserCredentials(string login, string password)
        {
            var user = await _Usercontext.Users
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
            return user != null;
        }
    }
}