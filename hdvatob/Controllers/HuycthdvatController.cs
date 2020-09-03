using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace hdvatob.Controllers
{
    public class HuycthdvatController : BaseController
    {
        private readonly IHuycthdvatRepository _huycthdvatRepository;
        private readonly IDmtkRepository _dmtkRepository;
        private readonly IHuyhdvatRepository _huyhdvatRepository;
        private readonly IDmhttcRepository _dmhttcRepository;
        private readonly IHoadonRepository _hoadonRepository;
        private readonly ICthdvatRepository _cthdvatRepository;
        string temp, log = "";
        string _username = "";

        public HuycthdvatController(IHuycthdvatRepository huycthdvatRepository, IDmtkRepository dmtkRepository, 
                                    IHuyhdvatRepository huyhdvatRepository, IDmhttcRepository dmhttcRepository,
                                    IHoadonRepository hoadonRepository, ICthdvatRepository cthdvatRepository )
        {
            _huycthdvatRepository = huycthdvatRepository;
            _dmtkRepository = dmtkRepository;
            _huyhdvatRepository = huyhdvatRepository;
            _dmhttcRepository = dmhttcRepository;
            _hoadonRepository = hoadonRepository;
            _cthdvatRepository = cthdvatRepository;
            _username = "huukhanh";
        }


        public ActionResult ListHuyCthdvat(string id)
        {
            var listct = _huycthdvatRepository.Listhuycthdvat(id,HttpContext.Session.GetString("chinhanh"));
            
            return PartialView(listct);
        }

        //#region Hiển thị thông tin chi tiết của Action lấy data từ vé tour
        //public ActionResult ListCtVetourBySerial(string Idhoadon, string tour, string serial, decimal? ppv, decimal? tygia, string tkno, string tkco)
        //{
        //    var vetour = _hoadonRepository.GetVetourBySerial(tour, serial);
        //    ViewBag.tongtien = vetour.sotiennt;
        //    var listct = _cthdvatRepository.ListCtVetourBySerial(tour, serial);
        //    List<cthdvat> newList = new List<cthdvat>();
        //    decimal number1 = 0;
        //    decimal number2 = 0;

        //    var adl = listct.Where(x => x.serial == serial).Where(x => x.dotuoi == "ADL").Select(x => x.sotiennt);
        //    var doanhthunnCHD = listct.Where(x => x.serial == serial).Where(x => x.dotuoi != "ADL").Select(x => x.sotiennt);
        //    var doanhthunnADL = listct.Where(x => x.serial == serial).Where(x => x.dotuoi.Equals("ADL")).Select(x => x.doanhthunn);
        //    number1 = adl.Sum();
        //    number2 = doanhthunnADL.Sum();
        //    var star1 = number1 - number2;
        //    var star2 = number2 + doanhthunnCHD.Sum();
        //    for (int j = 0; j < 2; j++)
        //    {
        //        cthdvat ct = new cthdvat();
        //        ct.Idhoadon = Idhoadon;
        //        ct.serial = vetour.serial;
        //        ct.diengiai = vetour.diengiai;
        //        ct.xuatve = Convert.ToDateTime(vetour.xuatve);
        //        ct.tenkhach = vetour.tenkhach;
        //        ct.sgtcode = vetour.sgtcode;
        //        if (j == 0)
        //        {
        //            ct.sotiennt = star1;
        //            ct.diengiai = "Vat 10% " + vetour.diengiai;
        //            ct.sotien = star1;
        //            ct.vat = 10;
        //        }
        //        else
        //        {
        //            ct.sotiennt = star2;
        //            ct.diengiai = vetour.diengiai;
        //            ct.sotien = star2;
        //            ct.vat = 0;

        //        }
        //        ct.ppv = Convert.ToDecimal(ppv);
        //        ct.tygia = (decimal)tygia;
        //        ct.ghichu = vetour.ghichu;
        //        ct.tkco = tkco;
        //        ct.tkno = tkno;
        //        ct.sttdong = 0;
        //        ct.ngaytao = System.DateTime.Now;
        //        ct.khachhuy = false;
        //        ct.httc = "";
        //        ct.dichvu = "";
        //        ct.loaitien = vetour.loaitien;
        //        ct.tour = tour;
        //        newList.Add(ct);


        //    }
        //    return View(newList);
        //}
        //#endregion

        #region Cập nhật huỷ chi tiết hoa đơn
        [HttpGet]
        public ActionResult Capnhathuycthd(decimal id)
        {
            var cthd = _huycthdvatRepository.GetById(id);
             var hd = _huyhdvatRepository.GetByTwoKey(cthd.Idhoadon, cthd.chinhanh);
            cthd.sotien = -cthd.sotien;
            cthd.sotiennt = -cthd.sotiennt;
            ViewBag.idhoadon = cthd.Idhoadon;
            ListNgoaite(cthd.loaitien);
            Listhttc(cthd.httc);
            Tkco(cthd.tkco);
            Tkno(cthd.tkno);
            if(string.IsNullOrEmpty(hd.keyhddt))
            {
                ViewBag.hide = "Cập nhật";
            }
            else
            {
                ViewBag.hide = "hide";
            }
            //var hd = _hoadonRepository.GetById(cthd.Idhoadon);
            //if (!String.IsNullOrEmpty(hd.hdvat))
            //{
            //    SetAlert("Đây là hoá đơn điện tử, không được điều chỉnh thông tin khi đã xuất hoá đơn", "error");
            //    return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
            //}
            return View("Capnhathuycthd", cthd);
        }

        [HttpPost]
        public ActionResult Capnhathuycthd(Huycthdvat entity)
        {
            temp = ""; log = "";
            
            Huycthdvat ct = _huycthdvatRepository.GetById(entity.Id);
            
            if (ct.diengiai != entity.diengiai)
            {
                temp += String.Format("- Diễn giải thay đổi: {0}->{1}", ct.diengiai, entity.diengiai);
            }
            if (!string.IsNullOrEmpty(ct.tenkhach) != !string.IsNullOrEmpty(entity.tenkhach))
            {
                temp += String.Format("- Tên khách thay đổi: {0}->{1}", ct.tenkhach, entity.tenkhach);
            }
            if (!string.IsNullOrEmpty(ct.sgtcode) != !string.IsNullOrEmpty(entity.sgtcode))
            {
                temp += String.Format("- Tour code thay đổi: {0}->{1}", ct.sgtcode, entity.sgtcode);
            }
            if (ct.sk != entity.sk)
            {
                temp += String.Format("- Số khách thay đổi: {0}->{1}", ct.sk, entity.sk);
            }
            if (Math.Abs(ct.sotiennt) != Math.Abs( entity.sotiennt))
            {
                temp += String.Format("- Tiền NT thay đổi: {0:#,##0.0}->{1:#,##0.0}", ct.sotiennt, entity.sotiennt);
            }
            if (ct.loaitien != entity.loaitien)
            {
                temp += String.Format("- Loại tiền thay đổi thay đổi: {0}->{1}", ct.loaitien, entity.loaitien);
            }
            if (ct.tygia != entity.tygia)
            {
                temp += String.Format("- Tỷ giá thay đổi: {0}->{1}", ct.tygia, entity.tygia);
            }
            if (Math.Abs(ct.sotien) != Math.Abs( entity.sotien))
            {
                temp += String.Format("- Tiền VNĐ thay đổi: {0:#,##0}->{1:#,##0}", ct.sotien, entity.sotien);
            }
            if (ct.ppv != entity.ppv)
            {
                temp += String.Format("- Phí phục vụ thay đổi: {0}->{1}", ct.ppv, entity.ppv);
            }
            
            if (!string.IsNullOrEmpty(ct.httc) != !string.IsNullOrEmpty(entity.httc))
            {
                temp += String.Format("- HTTC thay đổi: {0}->{1}", ct.httc, entity.httc);
            }
            if (ct.tkno != entity.tkno)
            {
                temp += String.Format("- Tài khoản nợ thay đổi: {0}->{1}", ct.tkno, entity.tkno);
            }
            if (ct.tkco != entity.tkco)
            {
                temp += String.Format("- Tài khoản có thay đổi: {0}->{1}", ct.tkco, entity.tkco);
            }
            if (ct.ghichu != entity.ghichu)
            {
                temp += String.Format("- Ghi chú thay đổi: {0}->{1}", ct.sgtcode, entity.sgtcode);
            }
            ct.diengiai = entity.diengiai;
            ct.tenkhach = entity.tenkhach;
            ct.sgtcode = entity.sgtcode;
            ct.sotiennt = - entity.sotiennt;
            ct.loaitien = entity.loaitien;
            ct.tygia = entity.tygia;
            ct.sotien =  - entity.sotien;
            ct.ppv = entity.ppv;
            ct.vat = entity.vat;
            ct.httc = entity.httc;
            ct.tkno = entity.tkno;
            ct.tkco = entity.tkco;
            ct.ghichu = entity.ghichu;
            ct.sk = entity.sk;
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User cập nhật huỷ chi tiết hoá đơn: " + _username + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ct.logfile = ct.logfile + log;
            }
            var result = _huycthdvatRepository.Update(ct);
            if (result != null)
            {
                SetAlert("Cập nhật chi tiết hoá đơn thành công", "success");
            }
            else
            {
                SetAlert("Cập nhật chi tiết hoá đơn không thành công", "error");
            }

            return Redirect(HttpContext.Session.GetString("urlEditHoadonhuy"));
        }


        #endregion

        #region Thêm huỷ chi tiết hoá đơn từ hoá đơn
        public ActionResult Themhuycthd(string idhoadon,string stt)
        {

            ViewBag.stt = stt;
            ViewBag.idhoadon = idhoadon;
            var hd = _hoadonRepository.getHoadonbyStt(stt);
            if (hd == null)
            {
                return View("Themhuycthd");
            }
            var ct = _cthdvatRepository.ListChitietHoadon(hd.Idhoadon, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = ct.Count();
            foreach (var c in ct)
            {
                c.diengiai = "DC GIAM HD " + hd.hdvat +" "+ c.diengiai;
                c.sotien = -c.sotien;
            }
            return View(ct);
        }
        [HttpPost]
        public ActionResult Themhuycthd_(string idhoadon, string listString)
        {
            var cthd = JsonConvert.DeserializeObject<List<cthdvat>>(listString);
            //List<Huycthdvat> a = new List<Huycthdvat>();
            foreach(var c in cthd)
            {
                var ct = _cthdvatRepository.GetById(c.Id);
                var hd = _hoadonRepository.GetByTwoKey(ct.Idhoadon,HttpContext.Session.GetString("chinhanh"));
                Huycthdvat h = new Huycthdvat();
                h.Idhoadon = idhoadon;
                h.chinhanh = HttpContext.Session.GetString("chinhanh");
                h.diengiai=  c.diengiai;
                h.sotien = c.sotien;
                h.serial = ct.serial;
                h.xuatve = ct.xuatve;
                h.tenkhach = ct.tenkhach;
                h.sgtcode = ct.sgtcode;
                h.sotiennt = -ct.sotiennt;
                h.sotien = -ct.sotien;
                h.loaitien = ct.loaitien;
                h.tygia = ct.tygia;
                h.ppv = ct.ppv;
                h.vat = ct.vat;
                h.ghichu = ct.ghichu;
                h.hoadonhuy = hd.hdvat;               
                h.coupon = ct.coupon;
                h.tiencoupon = ct.tiencoupon;
                h.tour = ct.tour;
                h.httc = ct.httc;
                h.tkno = ct.tkno;
                h.tkco = ct.tkco;
                h.ngaytao = System.DateTime.Now;
                h.logfile = "User thêm chi tiết huỷ: " + _username + " vào lúc " + System.DateTime.Now;
                _huycthdvatRepository.Create(h);
                ct.hoadonhuy = idhoadon;
                ct.ngayhuy = System.DateTime.Now;
                _cthdvatRepository.Update(ct);
            }

            return Redirect(HttpContext.Session.GetString("urlEditHoadonhuy"));
        }
        #endregion

        #region Thêm chi tiết huỷ bằng tay
        public ActionResult Themhuycthd_1(string idhoadon)
        {
            ViewBag.id = idhoadon;
            var ct = new Huycthdvat();
            ct.Idhoadon = idhoadon;
            ListNgoaite("VND");
            ListHttt("TM/CK");
            Listhttc("");
            Tkco("5113333336");
            Tkno("1311110000");
            ct.tygia = 1;
            var hd = _huyhdvatRepository.GetByTwoKey(ct.Idhoadon, HttpContext.Session.GetString("chinhanh"));
            if (!String.IsNullOrEmpty(hd.hdvat))
            {
                SetAlert("Đây là hoá đơn điện tử, không được điều chỉnh thông tin khi đã xuất hoá đơn", "error");
                return Redirect(HttpContext.Session.GetString("urlEditHoadonhuy"));
            }

            return View(ct);
        }
        [HttpPost]
        public ActionResult Themhuycthd_1(Huycthdvat entity)
        {
            entity.ngaytao = System.DateTime.Now;
            entity.tour = "OB";
            entity.chinhanh = HttpContext.Session.GetString("chinhanh");
            entity.serial = entity.serial ?? "";
            entity.hoadonhuy = entity.hoadonhuy ?? "";
            entity.httc = entity.httc ?? "";
            entity.diengiai = entity.diengiai ?? "";
            entity.sgtcode = entity.sgtcode ?? "";
            entity.tenkhach = entity.tenkhach ?? "";
            entity.sotien = -entity.sotien;
            entity.sotiennt = -entity.sotiennt;
            entity.logfile = "-User tạo chi tiết hoá đơn: " + _username + " vào lúc: " + System.DateTime.Now.ToString();
            var result = _huycthdvatRepository.Create(entity);
            if (result != null)
            {
                SetAlert("Thêm huỷ chi tiết hoá đơn thành công", "success");
            }
            else
            {
                SetAlert("Thêm huỷ chi tiết hoá đơn không thành công", "error");
            }

            return Redirect(HttpContext.Session.GetString("urlEditHoadonhuy"));
        }
        [HttpPost]
        public ActionResult updateSgtcode(decimal id, string sgtcode)
        {
            var cthd = _huycthdvatRepository.GetById(id);          
            temp = ""; log = "";
            if (!string.IsNullOrEmpty(sgtcode) != !string.IsNullOrEmpty(cthd.sgtcode))
            {
                temp += string.Format("- SGTcode thay đổi: {0}->{1}", cthd.sgtcode, sgtcode);

                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User cập nhật chi tiết hoá đơn: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                cthd.logfile = cthd.logfile + log;
            }
            cthd.sgtcode = string.IsNullOrEmpty(sgtcode) ? "" : sgtcode.ToUpper();
            var result = _huycthdvatRepository.Update(cthd);
            if (result != null)
            {
                SetAlert("Cập nhật Code đoàn thành công", "success");
            }
            else
            {
                SetAlert("Cập nhật Code đoàn không thành công", "error");
            }
            return Json(true);
        }
        #endregion

        #region Xoá huỷ chi tiết hoá đơn
        [HttpPost]
        public ActionResult Xoahuycthdvat(decimal id)
        {
            var huyct = _huycthdvatRepository.GetById(id);
            _huycthdvatRepository.Delete(huyct);
            return Json(true);
        }


        #endregion

        public ActionResult ViewlogHuycthoadon(decimal id)
        {
            var cthd =_huycthdvatRepository.GetById(id);
            return PartialView("ViewlogHuycthoadon", cthd);
        }

        #region List box
        public void ListHttt(string selected = "")
        {
            try
            {
                List<SelectListItem> httt = new List<SelectListItem>();
                httt.Add(new SelectListItem { Text = "TM/CK", Value = "TM/CK" });
                httt.Add(new SelectListItem { Text = "CK", Value = "CK" });
                httt.Add(new SelectListItem { Text = "TM", Value = "TM" });

                ViewBag.httt = new SelectList(httt, "Value", "Text", selected);
            }
            catch { return; }
        }
        public void ListNgoaite(string selected = "")
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
        public void Listhttc(string selected)
        {
            var a = _dmhttcRepository.ListHttc();
            IEnumerable<SelectListItem> selectList = from s in a
                                                     select new SelectListItem
                                                     {
                                                         Value = s.httc,
                                                         Text = string.IsNullOrEmpty(s.httc) ? "" : s.httc + " | " + s.diengiai.ToString()
                                                     };
            ViewBag.httc = new SelectList(selectList, "Value", "Text", selected);
        }
        #endregion
    }
}