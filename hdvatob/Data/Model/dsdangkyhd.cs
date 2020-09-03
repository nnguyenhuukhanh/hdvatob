using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class dsdangkyhd
    {
        [Key]
        public int id { get; set; }
        public string machinhanh { get; set; }
        public string tencn { get; set; }
        public string maviettat { get; set; }
        public string mausohd { get; set; }
        public string kyhieuhd { get; set; }
        public string masothue { get; set; }
        public decimal?  sohoadon { get; set; }
        public DateTime? ngaytaohd { get; set; }
        public string sohdtu { get; set; }
        public string sohdden { get; set; }
        public DateTime sudungtungay { get; set; }
        public DateTime sudungdenngay { get; set; }
        public decimal mainkey { get; set; }
        public string dienthoai { get; set; }
        public string diachi { get; set; }
        public bool activation { get; set; }
        //public string sitehddt { get; set; }
        //public string usersite { get; set; }
        //public string passsite { get; set; }
    }
}
