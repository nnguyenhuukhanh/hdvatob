using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.Data.Utilities;
using hdvatob.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace hdvatob.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IUserRepository _userRepository;
        MaHoaSHA1 sha1;
        public LoginController(ILoginRepository loginRepository, IUserRepository userRepository)
        {
            _loginRepository = loginRepository;
            _userRepository = userRepository;
            sha1 = new MaHoaSHA1();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                LoginModel result = _loginRepository.login(model.Username, model.Password, "011");
                if (result == null)
                {
                    ModelState.AddModelError("", "Tài khoản này không tồn tại");
                }
                else
                {
                    if (!result.trangthai)
                    {
                        ModelState.AddModelError("", "Tài khoản này không đã bị khoá");
                    }
                    string modelPass = sha1.EncodeSHA1(model.Password);
                    if (result.Password != modelPass)
                    {
                        ModelState.AddModelError("", "Mật khẩu không đúng");
                    }
                    else
                    {
                        var user = _userRepository.GetById(result.Username);//.FirstOrDefault();

                        HttpContext.Session.SetString("username", result.Username);
                        HttpContext.Session.SetString("password", model.Password);
                        HttpContext.Session.SetString("hoten", result.hoten);
                        HttpContext.Session.SetString("chinhanh", user.chinhanh);
                        HttpContext.Session.SetString("maviettat", user.maviettat);
                        HttpContext.Session.SetString("mausohd", model.mausohd);
                        HttpContext.Session.SetString("kyhieuhd", model.kyhieuhd);
                        HttpContext.Session.SetString("accounthddt", user.accounthddt);
                        HttpContext.Session.SetString("passhddt", user.passwordhddt);
                        HttpContext.Session.SetString("admin", user.isAdmin.ToString());
                        return RedirectToAction("Index", "Home");

                    }
                }

            }
            return View();
        }

        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult listMausohd(string username)
        {

            List<UserInfo> mausohd = _userRepository.getUserInfo(username).ToList();
            return Json(new
            {
                data = JsonConvert.SerializeObject(mausohd),
                status = true
            }
             );
        }
        public IActionResult changepass()
        {
            var user = _userRepository.GetById(HttpContext.Session.GetString("username"));

            changepassViewModel changpassmodel = new changepassViewModel
            {
                Username = user.username
            };
            
            return View(changpassmodel);
        }
        [HttpPost]
        public IActionResult changepass(changepassViewModel model)
        {
            if (ModelState.IsValid)
            {
                string oldpass = HttpContext.Session.GetString("password");
                if (sha1.EncodeSHA1(oldpass) != sha1.EncodeSHA1(model.Password))
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                }
                else if (model.Newpassword != model.Confirmpassword)
                {
                    ModelState.AddModelError("", "Mật khẩu nhập lại không đúng.");
                }
                else
                {
                    int result = _loginRepository.changePass(model.Username, sha1.EncodeSHA1(model.Newpassword));
                    if (result > 0)
                    {
                       
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể đổi mật khẩu.");
                    }
                }

            }
            return View();
        }

    }
}