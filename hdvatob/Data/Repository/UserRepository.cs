using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace hdvatob.Data.Repository
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        public UserRepository(hdvatobDbContext context) : base(context)
        {
        }

        
        public List<UserInfo> getUserInfo(string username)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@username",username)
             };
            var result = _context.UserInfos.FromSql("spGetUserInfo @username", parammeter).ToList();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public IPagedList<Users> ListUsers(string searchString, int? page)
        {
            if (page.HasValue && page < 1)
                return null;

            var list = _context.Users.AsQueryable();
            if (String.IsNullOrEmpty(searchString))
            {
                list = list.OrderBy(x=>x.username);
            }
            else
            {
                list = list.Where(x => x.username.Contains(searchString) || x.chinhanh.Contains(searchString) || x.maviettat.Contains(searchString)).OrderBy(x=>x.username);
            }

            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public int createUsers_qltaikhoan(string username, string hoten, string password, string chinhanh)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@username",username),
                    new SqlParameter("@hoten",hoten),
                    new SqlParameter("@password",password),
                    new SqlParameter("@chinhanh",chinhanh),
             };
            try
            {
                return _context.Database.ExecuteSqlCommand("spTaoNhanvienTrenQltk @username, @hoten, @password,@chinhanh ", parammeter);
            }
            catch
            {
                throw;
            }
        }
    }
}
