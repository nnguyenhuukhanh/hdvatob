using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.Data.Utilities;
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
    public class HoadonRepository : Repository<Hoadon>, IHoadonRepository
    {
        public HoadonRepository(hdvatobDbContext context) : base(context)
        {
        }
        GenerateId generateId = new GenerateId();
        public IPagedList<Hoadon> ListHoadon(string searchString, string chinhanh, int? page)
        {
            if (page.HasValue && page < 1)
                return null;

            //var list = _context.Hoadon.AsQueryable();
            //if (String.IsNullOrEmpty(searchString))
            //{
            //    list = list.Where(x => x.chinhanh == chinhanh  && x.ngayct >= System.DateTime.Now.AddMonths(-6)).OrderByDescending(x => x.Idhoadon);
            //}
            //else
            //{
            //    list = list.Where(x => x.chinhanh == chinhanh && (x.Idhoadon.Contains(searchString) || x.hdvat.Contains(searchString) || x.stt.Contains(searchString) || x.kyhieu.Contains(searchString) || x.makh.Contains(searchString) || x.tenkh.Contains(searchString))).OrderByDescending(x =>x.Idhoadon);
            //}
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@chinhanh",chinhanh) ,
                    new SqlParameter("@search",searchString)
               };
            List<Hoadon> list = _context.Hoadon.FromSql("spListHoadon @chinhanh, @search", parameter).ToList();

            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        //public string newStt(string  maviettat)
        //{
        //    maviettat = maviettat ?? "";
        //    var newsott = generateId.NextId(lastStt(maviettat), "", "000001") + maviettat + System.DateTime.Now.Year.ToString();
        //    return newsott;
        //}
        //public string lastStt(string maviettat)
        //{
        //    maviettat = maviettat ?? "";
        //    string last = "";
        //    try
        //    {
        //        if (maviettat.Length > 0)
        //        {
        //            last = _context.Hoadon.Where(x => x.stt.Substring(6, 11) == maviettat + System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
        //        }
        //        else
        //        {
        //            last = _context.Hoadon.Where(x => x.stt.Substring(6, 4) == System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
        //        }
        //        var a = last.Substring(0, 6);
        //        return last.Substring(0, 6);
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        public List<DataTuVetour> listdatavetour(string tour, string tungay, string denngay, string tuyentq, string chinhanh)
        {
            //if (chinhanh == "STN")
            //    chinhanh = "STS";
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tour",tour),
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@tuyentq",tuyentq),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.DataTuVetour.FromSql("spLayDataVetour_ @tour,@tungay,@denngay,@tuyentq,@chinhanh", parameter).ToList();
            return d;
        }
        
        //public List<DataFromTourViewModel> listDataFromTour(string tour, string tungay, string denngay, string tuyentq, string chinhanh)
        //{
            

        //    var parameter = new SqlParameter[]
        //       {
        //            new SqlParameter("@tour",tour),
        //            new SqlParameter("@tungay",tungay),
        //            new SqlParameter("@denngay",denngay),
        //            new SqlParameter("@tuyentq",tuyentq),
        //            new SqlParameter("@chinhanh",chinhanh)
        //       };
        //    var d = _context.DataFromTourViewModels.FromSql("spLayDataVetour_ @tour,@tungay,@denngay,@tuyentq,@chinhanh", parameter).ToList();
            
        //    return d;
        //}

        //public DataTuVetour GetvetourBySerial(string serial, string chinhanh)
        //{
        //    var parameter = new SqlParameter[]
        //       {
        //            new SqlParameter("@serial",serial),
        //            new SqlParameter("@chinhanh",chinhanh)
        //       };
        //    try
        //    {
        //        var d = _context.DataTuVetour.FromSql("spTimvetourByserial @serial ,@chinhanh", parameter).SingleOrDefault();
        //        return d;
        //    }
        //    catch { return null; }
        //}

        public string newhdvat(string kyhieu)
        {
            var newhd = generateId.NextId(lastHdvat(kyhieu), "", "0000001");
            return newhd;
        }
        public string lastHdvat(string kyhieu)
        {
            var parameter = new SqlParameter[]
                {
                    new SqlParameter("@kyhieu",kyhieu)
                };
            try
            {
                string d = _context.Hoadon.FromSql("spLasthoadon @kyhieu", parameter).SingleOrDefault().hdvat;
                return d;
            }
            catch
            {
                return "";
            }
        }

       

        public string newId(string chinhanh)
        {
            var newId = generateId.NextId(lastId(chinhanh), "", "000001") +  System.DateTime.Now.Year.ToString();
            return newId;
        }

        public string lastId(string chinhanh)
        {
            try
            {
                string last = "";
                last = _context.Hoadon.Where(x => x.chinhanh == chinhanh && x.Idhoadon.Substring(6, 4) == System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.Idhoadon).Take(1).SingleOrDefault().Idhoadon;
                //var a = last.Substring(0, 6);
                return last.Substring(0, 6);
            }
            catch
            {
                return "";
            }
        }

        public Hoadon getHoadonbyStt(string stt)
        {
            return _context.Hoadon.Where(x => x.stt == stt).SingleOrDefault();
        }

        //public List<ChitietVetourViewModel> listChitietVetour(string tour, string serial)
        //{
        //    var parameter = new SqlParameter[]
        //       {
        //            new SqlParameter("@tour",tour),
        //            new SqlParameter("@serial",serial)
        //       };
        //    var d = _context.ChitietVetourViewModel.FromSql("spLayChitietDataVetour @tour,@serial", parameter).ToList();

        //    return d;
        //}

        public VetourBySerial GetVetourBySerial(string tour, string serial,string chinhanh)
        {
            //if (chinhanh == "STN")
            //    chinhanh = "STS";
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@tour",tour),
                    new SqlParameter("@serial",serial),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            var d = _context.VetourBySerials.FromSql("spGetVetourBySerial @tour, @serial, @chinhanh", parameter).FirstOrDefault();
            return d;
        }

        public TienCoupon GetTienCoupon(string tour, decimal Idvetour)
        {
            try
            {
                var parameter = new SqlParameter[]
                  {
                    new SqlParameter("@tour",tour),
                    new SqlParameter("@idvetour",Idvetour)
                  };
                var d = _context.TienCoupons.FromSql("spTongCoupon @tour, @idvetour", parameter).FirstOrDefault();
                return d;
            }
            catch
            {
                return null;
            }
        }

        public int huyhoadontrongthang(string idhoadon, string chinhanh)
        {
            var parammeter = new SqlParameter[]
             {
                    new SqlParameter("@idhoadon",idhoadon),
                    new SqlParameter("@chinhanh",chinhanh)                   
             };
            try
            {
                return _context.Database.ExecuteSqlCommand("spHuyhoadontrongthang @idhoadon, @chinhanh", parammeter);
            }
            catch
            {
                throw;
            }
        }
    }
}
