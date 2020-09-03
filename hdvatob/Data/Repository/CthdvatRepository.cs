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
    public class CthdvatRepository : Repository<cthdvat>, ICthdvatRepository
    {
        public CthdvatRepository(hdvatobDbContext context) : base(context)
        {
        }

        public List<cthdvat> ListChitietHoadon(string Idhoadon,string chinhanh)
        {
            return _context.cthdvat.Where(x => x.Idhoadon == Idhoadon && x.chinhanh==chinhanh).OrderBy(x=>x.Id).ToList();           
        }

        public List<cthdvat> ListChitietHoadonhuy(string Idhoadon,string chinhanh)
        {
            return _context.cthdvat.Where(x => x.Idhoadon == Idhoadon && x.chinhanh==chinhanh).Where(x=>x.hoadonhuy !=null).OrderBy(x => x.Id).ToList();
        }

       
        public List<DataFromTourViewModel> ListCtVetourBySerial(string tour, string serial)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tour",tour),
                    new SqlParameter("@serial",serial)
               };
            var d = _context.DataFromTourViewModels.FromSql("spLayChitietDataVetour @tour,@serial", parameter).ToList();

            return d;
        }
    }
}
