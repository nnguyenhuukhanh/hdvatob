using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class SupplierViewModel
    {
        public string code { get; set; }
        public string chinhanh { get; set; }
        [Required(ErrorMessage = "Nhập tên giao dịch")]
        public string name { get; set; }
        [Required(ErrorMessage = "Nhập tên thương mại")]
        public string realname { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public string fax { get; set; }
        [Required(ErrorMessage = "Nhập Email")]
        //[EmailAddress(ErrorMessage = "E-mail không đúng")]
        public string email { get; set; }
        public string contact { get; set; }
        public string taxcode { get; set; }      
    }
}
