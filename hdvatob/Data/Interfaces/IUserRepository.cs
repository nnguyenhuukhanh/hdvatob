using hdvatob.Data.Model;
using hdvatob.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace hdvatob.Data.Interfaces
{
    public interface IUserRepository:IRepository<Users>
    {
        IPagedList<Users> ListUsers(string searchString, int? page);

        List<UserInfo> getUserInfo(string username);

        int createUsers_qltaikhoan(string username, string hoten, string password, string chinhanh);

    }
}
