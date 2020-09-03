using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập user name")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập password")]
        public string Password { get; set; }       
        public string hoten { get; set; }
        public string macn { get; set; }
        public string mact { get; set; }      
        public bool trangthai { get; set; }
        public bool doimk { get; set; }
        public DateTime ngaydoimk { get; set; }
    }
}
