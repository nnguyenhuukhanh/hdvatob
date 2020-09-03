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
    public class HuyhdvatRepository : Repository<Huyhdvat>, IHuyhdvatRepository
    {
        public HuyhdvatRepository(hdvatobDbContext context) : base(context)
        {
        }
        GenerateId generateId = new GenerateId();

        public IPagedList<Huyhdvat> Listhuyhoadon(string searchString, string chinhanh, int? page)
        {
            if (page.HasValue && page < 1)
                return null;

            var list = _context.Huyhdvat.AsQueryable();
            if (String.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.chinhanh == chinhanh && x.ngayct >= System.DateTime.Now.AddMonths(-6)).OrderByDescending(x => x.Idhoadon);
            }
            else
            {
                list = list.Where(x => x.chinhanh == chinhanh &&  (x.Idhoadon.Contains(searchString) || x.hdvat.Contains(searchString) || x.stt.Contains(searchString) || x.kyhieu.Contains(searchString) || x.makh.Contains(searchString) || x.tenkh.Contains(searchString))).OrderByDescending(x => x.Idhoadon);
            }

            const int pageSize = 10;
            var listPaged = list.ToPagedList(page ?? 1, pageSize);

            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public string newhdvat(string kyhieu)
        {
            throw new NotImplementedException();
        }

        public string newStt(string maviettat)
        {
            maviettat = maviettat ?? "";
            var newsott = generateId.NextId(lastStt(maviettat), "", "000001") + maviettat + System.DateTime.Now.Year.ToString();
            return newsott;
        }
        public string lastStt(string maviettat)
        {
            maviettat = maviettat ?? "";
            string last = "";
            try
            {
                if (maviettat.Length > 0)
                {
                    last = _context.Huyhdvat.Where(x => x.stt.Substring(6, 11) == maviettat + System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
                }
                else
                {
                    last = _context.Hoadon.Where(x => x.stt.Substring(6, 4) == System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
                }
                var a = last.Substring(0, 6);
                return last.Substring(0, 6);
            }
            catch
            {
                return "";
            }
        }

        public List<DataTuVetour> listdatatuhuytour(string tour, string tungay, string denngay, string tuyentq, string chinhanh)
        {
            if (chinhanh == "STN")
                chinhanh = "STS";
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tour",tour),
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@tuyentq",tuyentq),
                    new SqlParameter("@chinhanh",chinhanh)
              };
            var d = _context.DataTuVetour.FromSql("spLayDataTuhuytour @tour,@tungay,@denngay,@tuyentq,@chinhanh", parameter).ToList();
            return d;
        }

        public string newId()
        {
            var newId = generateId.NextId(lastId(), "", "000001") + System.DateTime.Now.Year.ToString();
            return newId;
        }

        public string lastId()
        {
            try
            {
                string last = "";
                last = _context.Huyhdvat.Where(x => x.Idhoadon.Substring(6, 4) == System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.Idhoadon).Take(1).SingleOrDefault().Idhoadon;
                var a = last.Substring(0, 6);
                return last.Substring(0, 6);
            }
            catch { return ""; }
        }
    }
}
