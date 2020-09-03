using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hdvatob.ViewModel
{
    public class changepassViewModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập password cũ")]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Vui lòng nhập password mới")]
        public string Newpassword { get; set; }
        [Display(Name = "Mật khẩu mới")]

        [Required(ErrorMessage = "Vui lòng nhập lại password mới")]
        public string Confirmpassword { get; set; }
    }
}
