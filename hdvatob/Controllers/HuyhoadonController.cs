using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace hdvatob.Controllers
{
    public class HuyhoadonController : BaseController
    {
        private readonly IHuyhdvatRepository _huyhdvatRepository;
        private readonly IHuycthdvatRepository _huycthdvatRepository;
        private readonly IDsdangkyhdRepository _dsdangkyhdRepository;
        private readonly IDmtkRepository _dmtkRepository;
        private readonly IHoadonRepository _hoadonRepository;
        private readonly INguonhdRepository _nguonhdRepository;
        string temp, log;//, _username, _chinhanh, _tour, _kyhieu = "",_maviettat = "";
        public HuyhoadonController(IHuyhdvatRepository huyhdvatRepository, IDsdangkyhdRepository dsdangkyhdRepository,
                                   IDmtkRepository dmtkRepository, IHoadonRepository hoadonRepository,IHuycthdvatRepository huycthdvatRepository,
                                   INguonhdRepository  nguonhdRepository)
        {
            _huyhdvatRepository = huyhdvatRepository;
            _huycthdvatRepository = huycthdvatRepository;
            _dsdangkyhdRepository = dsdangkyhdRepository;
            _dmtkRepository = dmtkRepository;
            _hoadonRepository = hoadonRepository;
            _nguonhdRepository = nguonhdRepository;
        }

        public IActionResult Index(string searchString,  int page = 1)
        {
            searchString = searchString ?? "";
           
            HttpContext.Session.SetString("urlHuyHoadon", UriHelper.GetDisplayUrl(Request));
            var huyhoadon = _huyhdvatRepository.Listhuyhoadon(searchString, HttpContext.Session.GetString("chinhanh"), page);
            ViewData["CurrentFilter"] = searchString;
            ViewBag.huyhoadon = huyhoadon;
            return View(huyhoadon);
        }
        #region Tạo hoá đơn huỷ
        public ActionResult Create()
        {
            var hd = new Huyhdvat();
            hd.ngayct = System.DateTime.Now;
            hd.kyhieu = HttpContext.Session.GetString("kyhieuhd").Trim();
            hd.mausohd = HttpContext.Session.GetString("mausohd").Trim();
            //var tthoadon = _dsdangkyhdRepository.getthongtinhd(HttpContext.Session.GetString("chinhanh"), HttpContext.Session.GetString("kyhieuhd"));
            //if (tthoadon != null)
            //{
            //    hd.kyhieu = tthoadon.kyhieuhd;
            //    hd.mausohd = tthoadon.mausohd;
            //    hd.stt = tthoadon.maviettat;
            //}
            // hd.stt = tthoadon.maviettat; _hoadonRepository.newStt(tthoadon.maviettat);
            listHttt("TM/CK");
            return View(hd);
        }

        [HttpPost]
        public ActionResult Create(Huyhdvat entity)
        {
            string maviettat = entity.stt;
            entity.Idhoadon = _huyhdvatRepository.newId();
            entity.ngaytao = System.DateTime.Now;
            string firstId = entity.Idhoadon.Substring(0, 6);
            string last = entity.Idhoadon.Substring(6, 4);
            entity.stt = firstId + HttpContext.Session.GetString("maviettat") + last;
            entity.chinhanh = HttpContext.Session.GetString("chinhanh");
            entity.nguoitaohd = HttpContext.Session.GetString("username");
            entity.logfile = " User tạo hoá đơn :" + entity.nguoitaohd + " vào lúc " + System.DateTime.Now.ToString();
            var result = _huyhdvatRepository.Create(entity);
            if (result == null)
            {
                SetAlert("Tạo hoá đơn không thành công. Vui lòng nhấn lại nút submit", "error");
                return View();
            }
            else
            {
                SetAlert("Tạo hoá đơn thành công", "success");
                return RedirectToAction("Edit", new { id = entity.Idhoadon });
            }

        }

        #endregion

        #region Cập nhật hoá đơn huỷ
        public IActionResult Edit(string id)
        {
            TempData["idhoadonhuy"] = id;
            HttpContext.Session.SetString("urlEditHoadonhuy", UriHelper.GetDisplayUrl(Request));
            if (id == null)
            {
                return NotFound();
            }
            var hoadon = _huyhdvatRepository.GetByTwoKey(id,HttpContext.Session.GetString("chinhanh"));
            if (hoadon == null)
            {
                return NotFound();
            }
            ViewBag.idhoadon = id;
            listHttt(hoadon.httt);
            if (string.IsNullOrEmpty(hoadon.hdvat) && hoadon.nguoitaohd == HttpContext.Session.GetString("username"))
            {
                ViewBag.capnhat = "Cập nhật";
            }
            else
            {
                ViewBag.capnhat = "hide";
            }
            return View(hoadon);
        }
        [HttpPost]
        public ActionResult Edit(Huyhdvat entity)
        {
            var hd = _huyhdvatRepository.GetByTwoKey(entity.Idhoadon,HttpContext.Session.GetString("chinhanh"));
            if (hd == null)
            {
                return NotFound();
            }
            if (hd.ngayct != entity.ngayct)
            {
                temp += String.Format("- Ngày HD thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", hd.ngayct, entity.ngayct);
            }
            if (hd.hdvat != entity.hdvat)
            {
                temp += String.Format("- Số hoá đơn VAT thay đổi: {0}->{1}", hd.hdvat, entity.hdvat);
            }
            if (hd.kyhieu != entity.kyhieu)
            {
                temp += String.Format("- Ký hiệu HĐ thay đổi: {0}->{1}", hd.kyhieu, entity.kyhieu);
            }
            if (hd.mausohd != entity.mausohd)
            {
                temp += String.Format("- Mẫu số HĐ thay đổi: {0}->{1}", hd.mausohd, entity.mausohd);
            }
            if (hd.makh != entity.makh)
            {
                temp += String.Format("- Mã KH thay đổi: {0}->{1}", hd.makh, entity.makh);
            }
            if (hd.tenkh != entity.tenkh)
            {
                temp += String.Format("- Tên cty thay đổi: {0}->{1}", hd.tenkh, entity.tenkh);
            }
            if (hd.tenkhach != entity.tenkhach)
            {
                temp += String.Format("- Tên khách thay đổi: {0}->{1}", hd.tenkhach, entity.tenkhach);
            }
            if (hd.coupon != entity.coupon)
            {
                temp += String.Format("- Coupon thay đổi: {0}->{1}", hd.coupon, entity.coupon);
            }
            if (hd.diachi != entity.diachi)
            {
                temp += String.Format("- Địa chỉ thay đổi: {0}->{1}", hd.diachi, entity.diachi);
            }
            if (hd.dienthoai != entity.dienthoai)
            {
                temp += String.Format("- Điện thoại thay đổi: {0}->{1}", hd.dienthoai, entity.dienthoai);
            }
            if (hd.msthue != entity.msthue)
            {
                temp += String.Format("- Mã số thuế thay đổi: {0}->{1}", hd.msthue, entity.msthue);
            }
            if (hd.httt != entity.httt)
            {
                temp += String.Format("- HTTT thay đổi: {0}->{1}", hd.httt, entity.httt);
            }
            if (hd.hopdong != entity.hopdong)
            {
                temp += String.Format("- Hợp đồng thay đổi: {0}->{1}", hd.hopdong, entity.hopdong);
            }
            if (hd.ghichu != entity.ghichu)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", hd.ghichu, entity.ghichu);
            }
            
            hd.ngayct = entity.ngayct;
            hd.hdvat = entity.hdvat;
            hd.kyhieu = entity.kyhieu;
            hd.mausohd = entity.mausohd;
            hd.makh = entity.makh;
            hd.tenkh = entity.tenkh;
            hd.tenkhach = entity.tenkhach;
            hd.coupon = entity.coupon;
            hd.diachi = entity.diachi;
            hd.dienthoai = entity.dienthoai;
            hd.msthue = entity.msthue;
            hd.httt = entity.httt;
            hd.hopdong = entity.hopdong;
            hd.ghichu = entity.ghichu;           
            hd.logfile = hd.logfile ?? "";
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User cập nhật hoá đơn vat: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                hd.logfile = hd.logfile + log;
            }
            var result = _huyhdvatRepository.Update(hd);
            if (result != null)
            {
                SetAlert("Cập nhật hoá đơn huỷ thành công", "success");
            }
            else
            {
                SetAlert("Cập nhật hoá đơn huỷ không thành công", "error");
            }
            return Redirect(HttpContext.Session.GetString("urlEditHoadonhuy"));
        }
        #endregion

        #region Lấy Data huỷ tour từ tour lẽ
        public ActionResult Datatuhuytour(string idhoadon, string tour, decimal? ppv, decimal? tygia, string tuyentq, string tungay, string denngay)
        {
            var hd = _huyhdvatRepository.GetByTwoKey(idhoadon,HttpContext.Session.GetString("chinhanh"));
           // tour = tour ?? _tour;
            ViewBag.tour = tour;
            tuyentq = tuyentq ?? "";
            tungay = tungay ?? hd.ngayct.Value.ToString("dd/MM/yyyy");
            denngay = denngay ?? hd.ngayct.Value.ToString("dd/MM/yyyy");
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            listDatatour(ViewBag.tour);
            Tkco("5113333336");
            Tkno("1311110000");
            ViewBag.chinhanh = HttpContext.Session.GetString("chinhanh");
            ViewBag.idhoadon = idhoadon;
            ViewBag.tuyentq = tuyentq;
            ViewBag.ppv = string.IsNullOrEmpty(ppv.ToString()) ? 0 : ppv;
            tygia = string.IsNullOrEmpty(tygia.ToString()) ? 1 : tygia;
            ViewBag.tygia = tygia;
            ViewBag.tuyentq = tuyentq;
            if (string.IsNullOrEmpty(tour))
            {
                return View("Datatuhuytour");
            }
            var d = _huyhdvatRepository.listdatatuhuytour(tour, tungay, denngay, tuyentq, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("Datatuhuytour");
            }
            else
            {
                return View("Datatuhuytour", d);
            }
        }
        [HttpPost]
        public ActionResult Datatuhuytour(string idhoadon, decimal? ppv, decimal? tygia, string tour, string tkno, string tkco, string list)
        {
            tygia = string.IsNullOrEmpty(tygia.ToString()) ? 1 : tygia;
            var idList = JsonConvert.DeserializeObject<List<DataTuVetour>>(list);
            foreach (var i in idList)
            {
                Huycthdvat ct = new Huycthdvat();
                ct.Idhoadon = idhoadon;
                ct.serial = i.serial;
                ct.diengiai = i.diengiai;
                ct.xuatve = Convert.ToDateTime(i.xuatve);
                ct.tenkhach = i.tenkhach;
                ct.sgtcode = i.sgtcode;
                ct.sotiennt = -((i.sotiennt - i.doanhthunn) * i.tygia);
                ct.ppv = Convert.ToDecimal(ppv);
                ct.vat = i.vat;
                ct.tygia = (decimal)tygia;
                ct.sotien = -((i.sotiennt - i.doanhthunn) * i.tygia);// Convert.ToDecimal(i.sotiennt * tygia);
                ct.ghichu = i.ghichu;
                ct.tkco = tkco;
                ct.tkno = tkno;
                ct.ngaytao = System.DateTime.Now;
                ct.khachhuy = false;
                ct.httc = "";
                ct.dichvu = "";
                ct.loaitien = i.loaitien;
                ct.tour = tour;
                ct.logfile = "===================" + System.Environment.NewLine + "User " + HttpContext.Session.GetString("username") + " thêm chi tiết từ tour " + tour + " vào lúc: " + System.DateTime.Now.ToString();
              //  _huycthdvatRepository.Create(ct);
            }

            return RedirectToAction("Edit", new { id = idhoadon });
        }
            #endregion

            #region Xem logfile hoá đơn
        public ActionResult Viewloghoadon(string idhoadon)
        {
            var hd = _huyhdvatRepository.GetByTwoKey(idhoadon,HttpContext.Session.GetString("chinhanh"));
            return PartialView("Viewloghoadon", hd);
        }
        #endregion

        #region List box
        public void listHttt(string selected = "")
        {
            try
            {
                List<SelectListItem> httt = new List<SelectListItem>();
                httt.Add(new SelectListItem { Text = "TM/CK", Value = "TM/CK" });
                httt.Add(new SelectListItem { Text = "CK", Value = "CK" });
                httt.Add(new SelectListItem { Text = "TM", Value = "TM" });

                ViewBag.httt = new SelectList(httt, "Text", "Value", selected);
            }
            catch { return; }
        }
        public void listNgoaite(string selected = "")
        {
            List<SelectListItem> ngoaite = new List<SelectListItem>();

            ngoaite.Add(new SelectListItem { Text = "VND", Value = "VND" });
            ngoaite.Add(new SelectListItem { Text = "USD", Value = "USD" });
            ngoaite.Add(new SelectListItem { Text = "EUR", Value = "EUR" });
            ngoaite.Add(new SelectListItem { Text = "SGD", Value = "SGD" });
            ngoaite.Add(new SelectListItem { Text = "AUD", Value = "AUD" });
            ngoaite.Add(new SelectListItem { Text = "GBP", Value = "GBP" });
            ViewBag.ngoaite = new SelectList(ngoaite, "Value", "Text", selected);
        }

        public void Tkno(string selected)
        {
            var a = _dmtkRepository.Listtaikhoan();
            IEnumerable<SelectListItem> selectList = from s in a
                                                     select new SelectListItem
                                                     {
                                                         Value = s.tkhoan,
                                                         Text = s.tkhoan + " | " + s.tentk.ToString()
                                                     };
            ViewBag.tkno = new SelectList(selectList, "Value", "Text", selected);
        }
        public void Tkco(string selected)
        {
            var a = _dmtkRepository.Listtaikhoan();
            IEnumerable<SelectListItem> selectList = from s in a
                                                     where s.tkhoan.StartsWith("511") || s.tkhoan.StartsWith("3331")
                                                     select new SelectListItem
                                                     {
                                                         Value = s.tkhoan,
                                                         Text = s.tkhoan + " | " + s.tentk.ToString()
                                                     };
            ViewBag.tkco = new SelectList(selectList, "Value", "Text", selected);
        }

        public void listDatatour(string selected = "")
        {
            try
            {
                var tour = _nguonhdRepository.GetAll().Where(x=>x.active==true);              
                ViewBag.tour = new SelectList(tour, "IdNguonhd", "IdNguonhd", selected);
            }
            catch { return; }
        }
        #endregion

    }
}