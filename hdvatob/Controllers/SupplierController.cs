using hdvatob.Data.Model;
using hdvatob.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using hdvatob.ViewModel;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.ServiceModel;
using Newtonsoft.Json;

namespace hdvatob.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IDsdangkyhdRepository _dsdangkyhdRepository;
        string temp = "", log = "";
        public SupplierController(ISupplierRepository supplierRepository, IDsdangkyhdRepository dsdangkyhdRepository)
        {
            _supplierRepository = supplierRepository;
            _dsdangkyhdRepository = dsdangkyhdRepository;
        }
        public IActionResult Index(string searchString, int page = 1)
        {
            searchString = searchString ?? "";
            //chinhanh = HttpContext.Session.GetString("chinhanh");// HttpContext.Session.GetString("chinhanh");
            HttpContext.Session.SetString("urlSupplier", UriHelper.GetDisplayUrl(Request));
            var supplier = _supplierRepository.ListSupplier(searchString, HttpContext.Session.GetString("chinhanh"), page);
            ViewData["CurrentFilter"] = searchString;
            ViewBag.supplier = supplier;
            ViewBag.username = HttpContext.Session.GetString("username");
            return View(supplier);
        }
        #region Tạo khách hàng
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(supplier entity)
        {
            if (ModelState.IsValid)
            {
                entity.code = entity.code.ToUpper();
                entity.taxcode = entity.taxcode ?? "";
                entity.email = entity.email ?? "";
                entity.name = entity.name.ToUpper();
                entity.realname = entity.realname.ToUpper();
                entity.logfile = "* User thêm thông tin khách hàng:" + HttpContext.Session.GetString("username") + " lúc " + System.DateTime.Now;
                entity.chinhanh = HttpContext.Session.GetString("chinhanh");
                entity.active = true;
                var result = _supplierRepository.Create(entity);
                if (result != null)
                {
                    SetAlert("Thêm khách hàng thành công", "success");
                    //return Redirect(HttpContext.Session.GetString("urlSupplier"));
                    return RedirectToAction("Edit", new { code = entity.code });
                }
                else
                {
                    SetAlert("Thêm khách hàng không thành công", "error");                   
                }
            }
            return View();
          
        }
        #endregion
        #region Cập nhật khách hàng
        public IActionResult Edit(string code)
        {
            HttpContext.Session.SetString("urlEditSupplier", UriHelper.GetDisplayUrl(Request));
            var s = _supplierRepository.GetByTwoKey(code, HttpContext.Session.GetString("chinhanh"));
            if (s == null)
            {
                return NotFound();
            }
            SupplierViewModel supplierViewModel = new SupplierViewModel
            {
                code = s.code,
                name = s.name,
                realname = s.realname,
                address = s.address,
                telephone = s.telephone,
                city=s.city,
                fax = s.fax,
                email = s.email,
                taxcode = s.taxcode,
                contact = s.contact
            };
            return View(supplierViewModel);
        }
        [HttpPost]
        public IActionResult Edit(supplier entity)
        {
            if (ModelState.IsValid)
            {
                temp = ""; log = "";
                var s = _supplierRepository.GetByTwoKey(entity.code, HttpContext.Session.GetString("chinhanh"));
                if (s == null)
                {
                    return NotFound();
                }
                if (s.name != entity.name)
                {
                    temp += string.Format("-Tên giao dịch thay đổi: {0}->{1}", s.name, entity.name);
                }
                if (s.realname != entity.realname)
                {
                    temp += string.Format("-Tên thương mại thay đổi: {0}->{1}", s.realname, entity.realname);
                }
                if (s.address != entity.address)
                {
                    temp += string.Format("-Địa chỉ thay đổi: {0}->{1}", s.address, entity.address);
                }
                if (s.city != entity.city)
                {
                    temp += string.Format("-Thành phố thay đổi: {0}->{1}", s.city, entity.city);
                }
                if (s.telephone != entity.telephone)
                {
                    temp += string.Format("-Điện thoại thay đổi: {0}->{1}", s.telephone, entity.telephone);
                }
                if (s.email != entity.email)
                {
                    temp += string.Format("-Email thay đổi: {0}->{1}", s.email, entity.email);
                }
                if (s.taxcode != entity.taxcode)
                {
                    temp += string.Format("-Mã số thuế thay đổi: {0}->{1}", s.taxcode, entity.taxcode);
                }
                if (temp.Length > 0)
                {
                    log = System.Environment.NewLine;
                    log += "=============";
                    log += System.Environment.NewLine;
                    log += temp + " -User cập nhật thông tin: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    s.logfile = s.logfile + log;
                }
                s.name = entity.name;
                s.realname = entity.realname;
                s.address = entity.address;
                s.city = entity.city;
                s.telephone = entity.telephone??"";
                s.email = entity.email??"";
                s.taxcode =  entity.taxcode??"";
                var result = _supplierRepository.Update(s);
                if (result != null)
                {
                    SetAlert("Cập nhật khách hàng thành công", "success");
                }
                else
                {
                    SetAlert("Cập nhật khách hàng không thành công", "error");
                }
            }
            return Redirect(HttpContext.Session.GetString("urlEditSupplier"));
        }
        #endregion

        #region Xoá khách hàng
        public IActionResult Xoakhachhang(string code)
        {
            if (code == null)
            {
                return NotFound();
            }
            var s = _supplierRepository.GetByTwoKey(code, HttpContext.Session.GetString("chinhanh"));
            if (s == null)
            {
                return NotFound();
            }
            s.active = false;
            s.logfile += s.logfile + System.Environment.NewLine + "====================" + System.Environment.NewLine + " * User xoá khách hàng: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now;
            _supplierRepository.Update(s);
            return Redirect(HttpContext.Session.GetString("urlSupplier"));
        }

        #endregion

        #region Publish khách hàng lên hoá đơn điện tử VNPT
        public async Task<IActionResult> TaoKhachhang(string code)
        {
            var s = _supplierRepository.GetByTwoKey(code, HttpContext.Session.GetString("chinhanh"));
            if (string.IsNullOrEmpty(s.email))
            {
                SetAlert("Vui lòng cập nhật email, sau đó hãy tạo khách hàng trên VNPT", "error");
                return Redirect(HttpContext.Session.GetString("urlEditSupplier"));
            }
            string xmlCusData = "<Customers>";
                xmlCusData += "<Customer>";
                    xmlCusData += "<Name>" + s.realname + "</Name>";
                    xmlCusData += "<Code>" + s.code.Trim() + HttpContext.Session.GetString("maviettat") + "</Code>";
                    xmlCusData += "<TaxCode>" + s.taxcode + "</TaxCode>";
                    xmlCusData += "<Address>" + s.address + "</Address>";
                    xmlCusData += "<BankAccountName></BankAccountName>";
                    xmlCusData += "<BankName></BankName>";
                    xmlCusData += "<BankNumber></BankNumber>";
                    xmlCusData += "<Email>" + s.email + "</Email>";
                    xmlCusData += "<Fax>" + s.fax + "</Fax>";
                    xmlCusData += "<Phone>" + s.telephone + "</Phone>";
                    xmlCusData += "<ContactPerson>" + s.contact + "</ContactPerson>";
                    xmlCusData += "<RepresentPerson></RepresentPerson>";
                    xmlCusData += "<CusType>1</CusType>";
                xmlCusData += "</Customer>";
            xmlCusData += "</Customers>";
            var inv = new PublishService.PublishServiceSoapClient(PublishService.PublishServiceSoapClient.EndpointConfiguration.PublishServiceSoap);

            var dkhd = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh==HttpContext.Session.GetString("maviettat")).SingleOrDefault();
            string sitehddt = dkhd.sitehddt.Trim() + "/PublishService.asmx";
            string usersite = dkhd.usersite;
            string passsite = dkhd.passsite;

            // Hàm add webservice động
            inv.ChannelFactory.Endpoint.Address = new EndpointAddress(sitehddt);

            Task<PublishService.UpdateCusResponse> ketqua = inv.UpdateCusAsync( xmlCusData, usersite, passsite, 0);// HttpContext.Session.GetString("masohd"), HttpContext.Session.GetString("kyhieuhd"), 0);
            
            var wait = await ketqua;
            int result = Convert.ToInt32(wait.Body.UpdateCusResult.ToString());
            if (result == -1)
            {
                SetAlert("Tài khoản không có quyền","error");
            }
            if (result == -2)
            {
                SetAlert("Không thêm được khách hàng trên hoá đơn điện tử", "error");
            }
            if (result == -3)
            {
                SetAlert("Dữ liệu không hợp lệ", "error");
            }
            if (result == -5)
            {
                SetAlert("Khách hàng đã tồn tại", "error");
            }
            if (result >0)
            {
                SetAlert("Tạo / cập nhật thông tin khách hàng trên hoá đơn điện tử thành công", "success");
            }

            return Redirect(HttpContext.Session.GetString("urlEditSupplier"));
        }
        #endregion

        #region Kiểm tra code mã khách hàng
        public IActionResult CodeExists(string code)
        {
         bool result = false;
            var s =  _supplierRepository.GetByTwoKey(code, HttpContext.Session.GetString("chinhanh"));
            if (s == null)
                result = true;

            return Json(result);
        }
        #endregion
        #region Xem log file khách hàng
        public ActionResult ViewlogKhachhang(string code)
        {
            var kh = _supplierRepository.GetByTwoKey(code, HttpContext.Session.GetString("chinhanh"));
            return PartialView("ViewlogKhachhang", kh);
        }
        #endregion

        #region Tìm thông tin khách
        public ActionResult listSearchkhach(string search)
        {
            search = search ?? "";
            var s = _supplierRepository.ListSupplierByCode(search, HttpContext.Session.GetString("chinhanh"));
            ViewBag.search = search;
            return PartialView(s);
        }
        public ActionResult listSearchkhach_i(string search)
        {
            search = search ?? "";
            var s = _supplierRepository.ListSupplierByCode(search, HttpContext.Session.GetString("chinhanh"));
            ViewBag.search = search;
            return PartialView(s);
        }
        // tìm mã khách hàng trong tách vé
        public ActionResult listSearchkhach_t(string search)
        {
            search = search ?? "";
            var s = _supplierRepository.ListSupplierByCode(search, HttpContext.Session.GetString("chinhanh"));
            ViewBag.search = search;
            return PartialView(s);
        }

        public ActionResult getSupplierbyId(string code)
        {
            var a = _supplierRepository.getSupplerByCode(code, HttpContext.Session.GetString("chinhanh"));
            var result = JsonConvert.SerializeObject(a);
            return Json(result);
        }

        #endregion
    }
}