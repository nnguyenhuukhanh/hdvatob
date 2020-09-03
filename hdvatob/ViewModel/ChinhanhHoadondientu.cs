using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class ChinhanhHoadondientu
    {
        [Key]
        [Required(ErrorMessage = "Nhập Mã CN")]
        [Remote("MacnExists", "Hoadondientu", ErrorMessage = "Mã CN đã có")]
        public string machinhanh { get; set; }
        [Required(ErrorMessage = "Nhập Tên chi nhánh")]
        public string tenchinhanh { get; set; }
        public string diachi { get; set; }
        public string dienthoai { get; set; }
        public string fax { get; set; }
        public string masothue { get; set; }
        public string maviettat { get; set; }
        public bool isActive { get; set; }
    }
}
