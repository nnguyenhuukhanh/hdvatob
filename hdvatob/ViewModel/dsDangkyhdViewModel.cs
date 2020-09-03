using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class dsDangkyhdViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Chinhanh { get; set; }
        public string Mausohd { get; set; }
        public string Kyhieuhd { get; set; }
        public string Masothue { get; set; }
        public decimal? Sohoadon { get; set; }
        public DateTime? Ngaytaohd { get; set; }
        public string Sohdtu { get; set; }
        public string Sohdden { get; set; }
        public DateTime Sudungtungay { get; set; }
        public DateTime Sudungdenngay { get; set; }
        public decimal Mainkey { get; set; }
        public string Dienthoai { get; set; }
        public string Diachi { get; set; }
        public bool Activation  { get; set; }
        public string Sitehddt { get; set; }
        public string Usersite { get; set; }
        public string Passsite { get; set; }
    }
}
