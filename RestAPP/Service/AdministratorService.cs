using Microsoft.EntityFrameworkCore;
using RestAPI.Context;
using RestAPI.Interfaces;
using RestAPI.Model;
using RestAPP.Context;

namespace RestAPI.Service
{
    public class AdministratorService : IAdministrator
    {
        private readonly AdminContext _Admincontext;

        public AdministratorService(AdminContext Admincontext)
        {
            _Admincontext = Admincontext;
        }
        public async Task ReginA(Administrator admin)
        {
            _Admincontext.Administrators.Add(admin);
            await _Admincontext.SaveChangesAsync();
        }

        public async Task<int> LoginAdmin(string login, string password)
        {
            Administrator admin = _Admincontext.Administrators.Where(x => x.Login == login && x.Password == password).First();
            return admin.Id;
        }
        
        public async Task<Administrator> GetAdminById(int id)
        {
            return await _Admincontext.Administrators.FindAsync(id);
        }

        public async Task<List<Administrator>> GetAllAdmins()
        {
            return await _Admincontext.Administrators.ToListAsync();
        }

        public async Task UpdateAdmin(Administrator admin)
        {
            _Admincontext.Administrators.Update(admin);
            await _Admincontext.SaveChangesAsync();
        }

        public async Task DeleteAdmin(int id)
        {
            var admin = await _Admincontext.Administrators.FindAsync(id);
            if (admin != null)
            {
                _Admincontext.Administrators.Remove(admin);
                await _Admincontext.SaveChangesAsync();
            }
        }
    }
}