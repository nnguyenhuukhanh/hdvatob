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
    public class DsdangkyhdRepository : Repository<dsdangkyhd>, IDsdangkyhdRepository
    {
        public DsdangkyhdRepository(hdvatobDbContext context) : base(context)
        {
        }

        public int capnhatDangkyhoadondientu(Dsdangkyhoadondientu entity)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@id",entity.id),
                    new SqlParameter("@chinhanh",entity.chinhanh??""),
                    new SqlParameter("@mausohd",entity.mausohd??""),
                    new SqlParameter("@kyhieuhd",entity.kyhieuhd??""),
                    new SqlParameter("@masothue",entity.masothue??""),
                    new SqlParameter("@sohoadon",entity.sohoadon.HasValue?entity.sohoadon.Value:0),
                    new SqlParameter("@sohdtu",entity.sohdtu??""),
                    new SqlParameter("@sohdden",entity.sohdden??""),
                    new SqlParameter("@sudungtungay",entity.sudungtungay),
                    new SqlParameter("@sudungdenngay",entity.sudungdenngay),
                     new SqlParameter("@mainkey",entity.mainkey),
                    new SqlParameter("@dienthoai",entity.dienthoai??""),
                    new SqlParameter("@diachi",entity.diachi??""),
                    new SqlParameter("@activation",entity.activation),
                    new SqlParameter("@sitehddt",entity.sitehddt??""),
                     new SqlParameter("@usersite",entity.usersite??""),
                    new SqlParameter("@passsite",entity.passsite??"")
               };
            return _context.Database.ExecuteSqlCommand("spcapnhatDangkyhoadondientu @id, @chinhanh,@mausohd,@kyhieuhd,@masothue,@sohoadon,@sohdtu,@sohdden,@sudungtungay,@sudungdenngay,@mainkey,@dienthoai,@diachi,@activation,@sitehddt,@usersite,@passsite", parameter);

        }

        public Dsdangkyhoadondientu getDkhdById(int id)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@id",id)
              };
            var d = _context.dsDangkyhoadondientu.FromSql("spGetdsdkhoadonById @id", parameter).FirstOrDefault();
            return d;
        }

        public dsdangkyhd getthongtinhd(string machinhanh,string kyhieu)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@machinhanh",machinhanh),
                    new SqlParameter("@kyhieu",kyhieu)
               };
            var d = _context.dsdangkyhd.FromSql("spLaythongtinhd @machinhanh, @kyhieu", parameter).SingleOrDefault();
            return d;
        }

        public List<ListDangkyHoadon> listDangkyhoadon()
        {
            var d = _context.listDangkyHoadons.FromSql("spListDsdangkyhoadon").ToList();
            return d;
        }

        public List<Dsdangkyhoadondientu> listDsDangkyhoadondientu()
        {
           
            var d = _context.dsDangkyhoadondientu.FromSql("spListDsdangkyhoadon").ToList();
            return d;
            //var d = _context.dsDangkyhoadondientu.FromSql("spListDsdangkyhoadon").ToList();
            //return d;
        }

        public int themDangkyhoadondientu(Dsdangkyhoadondientu entity)
        {
            var parameter = new SqlParameter[]
               {    
                    new SqlParameter("@chinhanh",entity.chinhanh??""),
                    new SqlParameter("@mausohd",entity.mausohd??""),
                    new SqlParameter("@kyhieuhd",entity.kyhieuhd??""),
                    new SqlParameter("@masothue",entity.masothue??""),
                    new SqlParameter("@sohoadon",entity.sohoadon.HasValue?entity.sohoadon.Value:0),
                    new SqlParameter("@sohdtu",entity.sohdtu??""),
                    new SqlParameter("@sohdden",entity.sohdden??""),
                    new SqlParameter("@sudungtungay",entity.sudungtungay),
                    new SqlParameter("@sudungdenngay",entity.sudungdenngay),
                    new SqlParameter("@mainkey",entity.mainkey),
                    new SqlParameter("@dienthoai",entity.dienthoai??""),
                    new SqlParameter("@diachi",entity.diachi??""),
                    new SqlParameter("@activation",entity.activation),
                    new SqlParameter("@sitehddt",entity.sitehddt??""),
                    new SqlParameter("@usersite",entity.usersite??""),
                    new SqlParameter("@passsite",entity.passsite??"")
               };
            return _context.Database.ExecuteSqlCommand("spThemDangkyhoadondientu @chinhanh,@mausohd,@kyhieuhd,@masothue,@sohoadon,@sohdtu,@sohdden,@sudungtungay,@sudungdenngay,@mainkey,@dienthoai,@diachi,@activation,@sitehddt,@usersite,@passsite", parameter);

        }

        public int updateMainkey(int id)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@id",id)
               };
            return _context.Database.ExecuteSqlCommand("spUpdateMainkey @id",parameter);
        }
        public int updateSohoadon(int id,decimal sohoadon)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@id",id),
                    new SqlParameter("@sohoadon",sohoadon)
               };
            return _context.Database.ExecuteSqlCommand("spUpdateSohoadon @id,@sohoadon", parameter);
        }
    }   
}
