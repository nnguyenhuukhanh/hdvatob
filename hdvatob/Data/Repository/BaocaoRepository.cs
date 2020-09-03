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
    public class BaocaoRepository : Repository<DoanhthungayhdViewModel>, IBaocaoRepository
    {
        public BaocaoRepository(hdvatobDbContext context) : base(context)
        {
        }

        public IEnumerable<DoanhthungayhdViewModel> listDoanhthungayhd(string tungay, string denngay,string tour, string chinhanh)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@tour",tour),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.doanhthungayhd.FromSql("spDoanthungayhoadon @tungay,@denngay,@tour, @chinhanh", parameter);
            return d;
        }

        public IEnumerable<NgayhuyhdViewModel> listNgayhuyhd(string tungay, string denngay,string tour, string chinhanh)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@tour",tour),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.Ngayhuyhd.FromSql("spNgayhuyhd @tungay,@denngay,@tour, @chinhanh", parameter);
            return d;
        }
    }
}
