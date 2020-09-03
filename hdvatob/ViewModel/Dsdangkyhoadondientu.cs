using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class Dsdangkyhoadondientu
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Nhập ký hiệu")]
        public string chinhanh { get; set; }
        [Required(ErrorMessage = "Nhập mẫu số hd")]
        public string mausohd { get; set; }
        [Required(ErrorMessage = "Nhập ký hiệu hd")]
        public string kyhieuhd { get; set; }            
        public string masothue { get; set; }
        public decimal? sohoadon { get; set; }
        public DateTime? ngaytaohd { get; set; }
        public string sohdtu { get; set; }
        public string sohdden { get; set; }
        public DateTime sudungtungay { get; set; }
        public DateTime sudungdenngay { get; set; }
        public decimal mainkey { get; set; }
        public string dienthoai { get; set; }
        public string diachi { get; set; }
        public bool activation { get; set; }
        [Required(ErrorMessage = "Nhập site hddt")]
        public string sitehddt { get; set; }
        [Required(ErrorMessage = "Nhập user hddt")]
        public string usersite { get; set; }
        [Required(ErrorMessage = "Nhập pass hddt")]
        public string passsite { get; set; }
    }
}
