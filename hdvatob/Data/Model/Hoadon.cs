using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class Hoadon
    {
        [Key]
        public string Idhoadon { get; set; }
        [Key]
        public string chinhanh { get; set; }
        public string stt { get; set; }
        public DateTime? ngayct { get; set; }
        public string hdvat { get; set; }
        public string kyhieu { get; set; }
        public string makh { get; set; }
        public string tenkh { get; set; }
        public string tenkhach { get; set; }
        public string diachi { get; set; }
        public string dienthoai { get; set; }
        public string msthue { get; set; }
        public string hopdong { get; set; }
        public string ghichu { get; set; }
        public string user { get; set; }
        public DateTime ngaytao { get; set; }
        public DateTime? ngayxoa { get; set; }
        public DateTime? ngayin { get; set; }
        public DateTime? capnhat { get; set; }
        public bool coupon { get; set; }
        public string httt { get; set; }
        public string serial { get; set; }
        public string keyhddt { get; set; }
        public string nguoitaohd { get; set; }
        public DateTime? datelock { get; set; }
        public string locker { get; set; }
        public string nguonhd { get; set; }
        public DateTime? batdau { get; set; }
        public DateTime? ketthuc { get; set; }
        public string mausohd { get; set; }
       
        public string logfile { get; set; }
    }
}
