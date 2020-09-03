using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Repository
{
    public class TachveRepository : Repository<Tachve>, ITachveRepository
    {
        public TachveRepository(hdvatobDbContext context) : base(context)
        {
        }
        GenerateId generateId = new GenerateId();
        //public string newStt(string maviettat)
        //{
        //    maviettat = maviettat ?? "";
        //    var newsott = generateId.NextId(lastStt(maviettat), "", "000001") + maviettat + System.DateTime.Now.Year.ToString();
        //    return newsott;
        //}
        public string lastStt(string maviettat)
        {
            maviettat = maviettat ?? "";
            string last = "";
            try
            {
                if (maviettat.Length > 0)
                {
                    last = _context.Tachve.Where(x => x.stt.Substring(6, 11) == maviettat + System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
                }
                else
                {
                    last = _context.Tachve.Where(x => x.stt.Substring(6, 4) == System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.stt).Take(1).SingleOrDefault().stt;
                }
                var a = last.Substring(0, 6);
                return last.Substring(0, 6);
            }
            catch
            {
                return "";
            }
        }

        public DataTuVetour TachvetourBySerial(string serial, string chinhanh)
        {
            var parameter = new SqlParameter[]
               {
                    new SqlParameter("@serial",serial),
                    new SqlParameter("@chinhanh",chinhanh)
               };
            try
            {
                var d = _context.DataTuVetour.FromSql("spTachvetourByserial @serial ,@chinhanh", parameter).SingleOrDefault();
                return d;
            }
            catch { throw; }
        }

        public List<Tachve> listHoadontach(string tungay, string denngay,string chinhanh)
        {
            var parameter = new SqlParameter[]
              {
                    new SqlParameter("@tungay",tungay),
                    new SqlParameter("@denngay",denngay),
                    new SqlParameter("@chinhanh",chinhanh)
              };
            try
            {
                var d = _context.Tachve.FromSql("splistHoadontach @tungay ,@denngay, @chinhanh", parameter).ToList();
                return d;
            }
            catch { return null; }
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
                last = _context.Tachve.Where(x => x.Idhoadon.Substring(6, 4) == System.DateTime.Now.Year.ToString()).OrderByDescending(x => x.Idhoadon).Take(1).SingleOrDefault().Idhoadon;
                var a = last.Substring(0, 6);
                return last.Substring(0, 6);
            }
            catch { return ""; }
        }
    }
}
