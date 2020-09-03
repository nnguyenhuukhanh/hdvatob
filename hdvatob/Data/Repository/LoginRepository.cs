using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Repository
{
    public class LoginRepository : Repository<LoginModel>, ILoginRepository
    {
        public LoginRepository(hdvatobDbContext context) : base(context)
        {
        }

        public int changePass(string username, string newPass)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@username",username),
                    new SqlParameter("@newpass",newPass)
             };
            try
            {
                return _context.Database.ExecuteSqlCommand("spChangepass @username, @newpass", parammeter);
            }
            catch
            {
                throw;
            }
        }

        public LoginModel login(string username, string password, string mact)
        {
            var parammeter = new SqlParameter[]
         {
                new SqlParameter("@username",username),
                new SqlParameter("@mact",mact)
         };

            var result = _context.LoginModel.FromSql("spLogin @username, @mact", parammeter).SingleOrDefault();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }
    }
    
}
