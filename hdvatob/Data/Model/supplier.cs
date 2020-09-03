using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class supplier
    {
        [Key]
        [Required(ErrorMessage = "Nhập mã kh")]
        [Remote("CodeExists", "supplier", ErrorMessage = "mã kh đã có")]
        public string code { get; set; }
        [Key]
        public string chinhanh { get; set; }
        [Required(ErrorMessage = "Nhập tên giao dịch")]
        public string name { get; set; }
        [Required(ErrorMessage = "Nhập tên thương mại")]
        public string realname { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string telephone { get; set; }
        public string fax { get; set; }
        //[DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng")]
        [Required(ErrorMessage = "Nhập Email")]
        //[EmailAddress(ErrorMessage = "E-mail không đúng")]
        public string email { get; set; }
        public string contact { get; set; }
        public DateTime? date { get; set; }
        public string field { get; set; }
        public string suppliercode { get; set; }
        public string paymentcode { get; set; }
        public int  room { get; set; }
        public int level { get; set; }
        public string website { get; set; }
        public string nation { get; set; }
        public string taxcode { get; set; }
        public string taxsign { get; set; }
        public string taxform { get; set; }
        public string httt { get; set; }
        public bool? muave { get; set; }
        public bool? daily { get; set; }
        public bool active { get; set; }
        public string logfile { get; set; }
    }
}
