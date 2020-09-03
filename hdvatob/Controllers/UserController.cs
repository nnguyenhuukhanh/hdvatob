using System;
using System.Threading.Tasks;
using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.Data.Utilities;
using hdvatob.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace hdvatob.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IDmChinhanhRepository _dmChinhanhRepository;
        private readonly IDsdangkyhdRepository _dsdangkyhdRepository;
        string temp, log = "";
        MaHoaSHA1 sha1;

        public UserController(IUserRepository userRepository, IDmChinhanhRepository dmChinhanhRepository, IDsdangkyhdRepository dsdangkyhdRepository )
        {
            _userRepository = userRepository;
            _dmChinhanhRepository = dmChinhanhRepository;
            _dsdangkyhdRepository = dsdangkyhdRepository;
            sha1 = new MaHoaSHA1();
        }
        public IActionResult Index(string searchString, int page = 1)
        {
            searchString = searchString ?? "";           
            HttpContext.Session.SetString("urlUsers", UriHelper.GetDisplayUrl(Request));
            var users = _userRepository.ListUsers(searchString, page);
            ViewData["CurrentFilter"] = searchString;
            ViewBag.users = users;
            //ViewBag.username = HttpContext.Session.GetString("username");
            return View(users);
        }
        #region Cập nhật nhân viên
        public IActionResult Edit(string id)
        {
            HttpContext.Session.SetString("urlEditUsers", UriHelper.GetDisplayUrl(Request));
            if (id == null)
            {
                return NotFound();
            }
            var u = _userRepository.GetById(id);
            if (u == null)
            {
                return NotFound();
            }
           
            UsersViewModel usersViewModel = new UsersViewModel
            {
                username = u.username,
                hoten = u.hoten,
                accounthddt = u.accounthddt,
                passwordhddt = u.passwordhddt,
                chinhanh= u.chinhanh,
                maviettat = u.maviettat,
                isAdmin = u.isAdmin
            };
            GetListChinhanh(usersViewModel.chinhanh);
            GetListDsDangkyhd(usersViewModel.maviettat);
            return View(usersViewModel);
        }
        [HttpPost]
        public IActionResult Edit(UsersViewModel entity)
        {
            temp = ""; log = "";
            var u = _userRepository.GetById(entity.username);
            if (u == null)
            {
                return NotFound();
            }
            
            if (u.hoten != entity.hoten)
            {
                temp += String.Format("- Họ tên thay đổi: {0}->{1}", u.hoten, entity.hoten);
            }
            if (u.accounthddt != entity.accounthddt)
            {
                temp += String.Format("- Account hoá đơn điện tử thay đổi: {0}->{1}", u.accounthddt, entity.accounthddt);
            }
            if (u.passwordhddt != entity.passwordhddt)
            {
                temp += String.Format("- Password hoá đơn điện tử thay đổi: {0}->{1}", u.passwordhddt, entity.passwordhddt);
            }
            if (u.maviettat != entity.maviettat)
            {
                temp += String.Format("- Ký hiệu thay đổi: {0}->{1}", u.maviettat, entity.maviettat);
            }
            if (u.chinhanh != entity.chinhanh)
            {
                temp += String.Format("- Quyền Admin thay đổi: {0}->{1}", u.chinhanh, entity.chinhanh);
            }
            if (u.isAdmin != entity.isAdmin)
            {
                temp += String.Format("- Account hoá đơn điện tử thay đổi: {0}->{1}", u.isAdmin, entity.isAdmin);
            }
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User cập nhật thông tin: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                u.logfile = u.logfile + log;
            }
            u.hoten = entity.hoten;
            u.accounthddt = entity.accounthddt;
            u.passwordhddt = entity.passwordhddt;
            u.maviettat = entity.maviettat;
            u.isAdmin = entity.isAdmin;
            u.chinhanh = entity.chinhanh;
            var result =_userRepository.Update(u);
            if (result != null)
            {
                SetAlert("Cập nhật nhân viên thành công", "success");
            }
            else
            {
                SetAlert("Cập nhật nhân viên không thành công", "error");
            }
            return Redirect(HttpContext.Session.GetString("urlEditUsers"));
        }
        public IActionResult CapnhatAccountVNPT ()
        {
            var url = UriHelper.GetDisplayUrl(Request);// HttpContext.Session.SetString("url", UriHelper.GetDisplayUrl(Request));
            var user = _userRepository.GetById(HttpContext.Session.GetString("username"));
            return PartialView(user);
        }
        [HttpPost]
        public IActionResult CapnhatAccountVNPT(Users entity)       
        {
            var u = _userRepository.GetById(entity.username);
            u.hoten = entity.hoten;
            u.accounthddt = entity.accounthddt;
            u.passwordhddt = entity.passwordhddt;
            var result = _userRepository.Update(u);
            if (result != null)
            {
                SetAlert("Cập nhật thông tin thành công", "success");
            }
            else
            {
                SetAlert("Cập nhật thông tin không thành công", "error");
            }
            return RedirectToAction("index", "Home");
           // return Redirect(HttpContext.Session.GetString("url"));
        }

        #endregion
        #region Thêm nhân viên
        public IActionResult Create()
        {
            GetListChinhanh(HttpContext.Session.GetString("chinhanh"));
            GetListDsDangkyhd("");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Users entity)
        {
            if (ModelState.IsValid)
            {
                entity.password = sha1.EncodeSHA1(entity.password);
                entity.accounthddt = string.IsNullOrEmpty(entity.accounthddt) ? "" : entity.accounthddt;
                entity.passwordhddt = string.IsNullOrEmpty(entity.passwordhddt) ? "" : entity.passwordhddt;
                entity.nguoitao = HttpContext.Session.GetString("username");
                entity.ngaytao = System.DateTime.Now;
                entity.logfile = "* User tạo nhân viên: " + HttpContext.Session.GetString("username");
                var result = _userRepository.Create(entity);
                int n = _userRepository.createUsers_qltaikhoan(entity.username, entity.hoten, entity.password, entity.chinhanh);
                if (result != null)
                {
                    SetAlert("Tạo nhân viên thành công.", "success");
                    return Redirect(HttpContext.Session.GetString("urlUsers"));
                }
                else
                {
                    SetAlert("Tạo nhân viên không thành công.", "error");                   
                }
            }
            return View();
        }
        #endregion
        #region Xoá nhân viên
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            var result = _userRepository.Delete(user);
            if (result == null)
            {
                SetAlert("Xóa nhân viên không thành công", "error");
            }
            else
            {
                SetAlert("Xóa nhân viên thành công", "success");
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Xem logfile
        public ActionResult ViewlogNhanvien(string username)
        {
            var u = _userRepository.GetById(username);
            return PartialView("ViewlogNhanvien", u);
        }

        #endregion

        #region Kiểm tra sự tồn tại của nhân viên
        public async Task<IActionResult> UserExists(string username)
        {
            bool result = false;
            var u = await _userRepository.GetByIdAsync(username);
            if (u == null)
                result = true;

            return Json(result);
        }
        #endregion
        #region Listbox
        public void GetListChinhanh(string selected = "")
        {
            try
            {
                var chinhanh = _dmChinhanhRepository.GetListChinhanh();
                ViewBag.chinhanh = new SelectList(chinhanh, "machinhanh", "machinhanh", selected);
            }
            catch { return; }
        }
        public void GetListDsDangkyhd(string selected = "")
        {
            try
            {
                var kyhieu = _dsdangkyhdRepository.listDangkyhoadon();              
                ViewBag.kyhieu = new SelectList(kyhieu, "chinhanh", "chinhanh", selected);
            }
            catch { return; }
        }
        #endregion
    }
}