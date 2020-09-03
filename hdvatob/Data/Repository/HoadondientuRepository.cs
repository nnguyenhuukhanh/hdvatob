using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace hdvatob.Data.Repository
{
    public class HoadondientuRepository : Repository<ChinhanhHoadondientu>, IHoadondientuRepository
    {
        public HoadondientuRepository(hdvatobDbContext context) : base(context)
        {

        }

        public int capnhatChinhanhHoadondientu(ChinhanhHoadondientu entity)
        {
            try
            {
                var parameter = new SqlParameter[]
               {
                    new SqlParameter("@machinhanh",entity.machinhanh.ToString()),
                    new SqlParameter("@tenchinhanh",entity.tenchinhanh??""),
                    new SqlParameter("@diachi",entity.diachi??""),
                    new SqlParameter("@dienthoai",entity.dienthoai??""),
                    new SqlParameter("@fax",entity.fax??""),
                    new SqlParameter("@masothue",entity.masothue??""),
                    new SqlParameter("@maviettat",entity.maviettat??""),
                    new SqlParameter("@isactive",entity.isActive)
               };

               int n = _context.Database.ExecuteSqlCommand("spCapnhatChinhanhHoadondientu @machinhanh, @tenchinhanh, @diachi, @dienthoai ,@fax , @masothue ,@maviettat, @isactive", parameter);               
                return n;
            }
            catch
            {
                throw ;
            }
        }

        public List<ChinhanhHoadondientu> dsChinhanhHoadondientu()
        {
            return _context.dsChinhanhHoadondientu.FromSql("spListDsChinhanhHoadondientu").ToList();
        }

        public int themChinhanhHoadondientu(ChinhanhHoadondientu entity)
        {
            var parameter = new SqlParameter[]
              {
                   new SqlParameter("@machinhanh",entity.machinhanh.ToString()),
                    new SqlParameter("@tenchinhanh",entity.tenchinhanh??""),
                    new SqlParameter("@diachi",entity.diachi??""),
                    new SqlParameter("@dienthoai",entity.dienthoai??""),
                    new SqlParameter("@fax",entity.fax??""),
                    new SqlParameter("@masothue",entity.masothue??""),
                    new SqlParameter("@maviettat",entity.maviettat??""),
                    new SqlParameter("@isactive",entity.isActive)
              };
            return _context.Database.ExecuteSqlCommand("spThemChinhanhHoadondientu @machinhanh, @tenchinhanh, @diachi, @dienthoai,@fax, @masothue,@maviettat,@isactive", parameter);
        }
    }
}
