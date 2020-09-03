using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.Data.Model
{
    public class Users
    {
        [Key]
        [Required(ErrorMessage = "Nhập username")]
        [Remote("UserExists", "User", ErrorMessage = "Username đã có")]
        public string username { get; set; }
        public string hoten { get; set; }
        public string password { get; set; }
        //[Required(ErrorMessage = "Nhập account VNPT")]
        public string accounthddt { get; set; }
        //[Required(ErrorMessage = "Nhập pass VNPT")]
        public string passwordhddt { get; set; }       
        public string maviettat { get; set; }
        public string chinhanh { get; set; }
        public bool  isAdmin { get; set; }
        public string logfile { get; set; }
        public DateTime? ngaytao { get; set; }
        public string nguoitao { get; set; }
    }
}
