using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class cthdvat
    {
        [Key]
        public decimal Id { get; set; }
        public string Idhoadon { get; set; }
        public string chinhanh { get; set; }
        public string diengiai { get; set; }
        public string serial { get; set; }
        public DateTime? xuatve { get; set; }
        public string tenkhach { get; set; }
        public string sgtcode { get; set; }
        public decimal sotiennt { get; set; }
        public string loaitien { get; set; }
        public decimal tygia { get; set; }
        public decimal sotien { get; set; }
        public decimal ppv { get; set; }
        public int vat { get; set; }
        public string ghichu { get; set; }
        public DateTime? ngaytao { get; set; }
        public DateTime? capnhat { get; set; }
        public DateTime? ngayhuy { get; set; }
        public string hoadonhuy { get; set; }
        public decimal coupon { get; set; }
        public decimal tiencoupon { get; set; }
        public bool khachhuy { get; set; }
        public string tkco { get; set; }
        public string tkno { get; set; }
        public string dichvu { get; set; }
        public DateTime? datelock { get; set; }
        public string locker { get; set; }
        public string tour { get; set; }
        public string httc { get; set; }
        public string number { get; set; }
        public int slve { get; set; }
        public int sk { get; set; }
        public int sttdong { get; set; }
        public string logfile { get; set; }
    }
}
