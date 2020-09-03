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
    public class DmChinhanhRepository : Repository<dmChinhanh>, IDmChinhanhRepository
    {
        public DmChinhanhRepository(hdvatobDbContext context) : base(context)
        {
        }

        public dmChinhanh getChinhanhById(string macn)
        {
            var parammeter = new SqlParameter[]
        {
                new SqlParameter("@macn",macn)
        };

            var result = _context.dmChinhanh.FromSql("spGetThongtinChinhanh @macn", parammeter).SingleOrDefault();
            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public List<ListChinhanh> GetListChinhanh()
        {
            return _context.GetListChinhanhs.FromSql("spListChinhanh").ToList();
        }

       
    }
}
