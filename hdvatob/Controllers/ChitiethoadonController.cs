using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hdvatob.Data.Model;
using hdvatob.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace hdvatob.Controllers
{
    public class ChitiethoadonController : BaseController
    {
        //private readonly hdvatobDbContext _context;
        private readonly ICthdvatRepository _cthdvatRepository;
        private readonly IDmtkRepository _dmtkRepository;
        private readonly IDmhttcRepository _dmhttcRepository;
        private readonly IHoadonRepository _hoadonRepository;
        string temp, log = "";


        public ChitiethoadonController(ICthdvatRepository cthdvatRepository, IDmtkRepository dmtkRepository,
                                       IDmhttcRepository dmhttcRepository, IHoadonRepository hoadonRepository)
        {
            _cthdvatRepository = cthdvatRepository;
            _dmtkRepository = dmtkRepository;
            _dmhttcRepository = dmhttcRepository;
            _hoadonRepository = hoadonRepository;
        }

        public ActionResult ListCthd(string id)
        {
            var listct = _cthdvatRepository.ListChitietHoadon(id, HttpContext.Session.GetString("chinhanh"));
            var hd = _hoadonRepository.GetByTwoKey(id, HttpContext.Session.GetString("chinhanh"));
            ViewBag.nguoitao = hd.nguoitaohd;
            ViewBag.user = HttpContext.Session.GetString("username");
            ViewBag.keyhddt = hd.keyhddt;

            return PartialView(listct);
        }
        public ActionResult ListCthd_(string id)
        {
            var listct = _cthdvatRepository.ListChitietHoadon(id, HttpContext.Session.GetString("chinhanh"));          
            return PartialView(listct);
        }

        #region Hiển thị thông tin chi tiết của Action lấy data từ vé tour
        public ActionResult ListCtVetourBySerial(string Idhoadon, string tour, string serial, decimal? ppv, decimal tygia, string tkno, string tkco)
        {
            var vetour = _hoadonRepository.GetVetourBySerial(tour, serial, HttpContext.Session.GetString("chinhanh"));

            List<cthdvat> newList = new List<cthdvat>();
            // Tính tổng cộng tiền coupon đặt cọc của vé tour
            var tccoupon = _hoadonRepository.GetTienCoupon(tour, vetour.id);
            if (tccoupon != null) // Nếu tiền coupon > 0 
            {
                cthdvat ct = new cthdvat();
                ct.Idhoadon = Idhoadon;
                ct.serial = vetour.serial;
                ct.diengiai = vetour.diengiai;
                ct.xuatve = Convert.ToDateTime(vetour.xuatve);
                ct.tenkhach = vetour.tenkhach;
                ct.sgtcode = vetour.sgtcode;
                // số tiền còn lại = số tiền vé tour - đặt cọc bằng coupon và vat =0 vì coupon đã tính thuế
                //ct.sotien = (vetour.sotiennt) ;
                //ct.sotiennt = (vetour.doanhthunn *tygia);
                ViewBag.tongtien = vetour.sotiennt - tccoupon.Coupon;
                if (tour == "OB")
                {
                    if (vetour.sotiennt - tccoupon.Coupon > (vetour.doanhthunn * tygia))
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if (j == 0)
                            {
                                ct.sotien = vetour.sotiennt - tccoupon.Coupon - (vetour.doanhthunn * tygia);
                                ct.sotiennt = vetour.sotiennt - tccoupon.Coupon - (vetour.doanhthunn * tygia);
                                ct.vat = 10;
                            }
                            else
                            {
                                ct.sotien = vetour.sotiennt - tccoupon.Coupon;
                                ct.sotien = vetour.sotiennt - tccoupon.Coupon;
                                ct.vat = 0;
                            }
                            ct.ppv = Convert.ToDecimal(ppv);
                            ct.tygia = (decimal)tygia;
                            ct.ghichu = vetour.ghichu;
                            ct.tkco = tkco;
                            ct.tkno = tkno;
                            ct.sttdong = 0;
                            ct.ngaytao = System.DateTime.Now;
                            ct.khachhuy = false;
                            ct.httc = "";
                            ct.dichvu = "";
                            ct.loaitien = vetour.loaitien;
                            ct.tour = tour;
                            ct.sk = vetour.sk;
                            newList.Add(ct);
                        }
                    }
                    else
                    {
                        ct.sotien = vetour.sotiennt - tccoupon.Coupon;
                        ct.sotien = vetour.sotiennt - tccoupon.Coupon;
                        ct.vat = 0;

                        ct.ppv = Convert.ToDecimal(ppv);
                        ct.tygia = (decimal)tygia;
                        ct.ghichu = vetour.ghichu;
                        ct.tkco = tkco;
                        ct.tkno = tkno;
                        ct.sttdong = 0;
                        ct.ngaytao = System.DateTime.Now;
                        ct.khachhuy = false;
                        ct.httc = "";
                        ct.dichvu = "";
                        ct.loaitien = vetour.loaitien;
                        ct.tour = tour;
                        ct.sk = vetour.sk;
                        newList.Add(ct);
                    }
                }
                else
                {
                    ct.sotien = vetour.sotiennt - tccoupon.Coupon;
                    ct.sotiennt = vetour.sotiennt - tccoupon.Coupon;
                    ct.vat = 10;
                    ct.ppv = Convert.ToDecimal(ppv);
                    ct.tygia = (decimal)tygia;
                    ct.ghichu = vetour.ghichu;
                    ct.tkco = tkco;
                    ct.tkno = tkno;
                    ct.sttdong = 0;
                    ct.ngaytao = System.DateTime.Now;
                    ct.khachhuy = false;
                    ct.httc = "";
                    ct.dichvu = "";
                    ct.loaitien = vetour.loaitien;
                    ct.tour = tour;
                    ct.sk = vetour.sk;
                    newList.Add(ct);
                }

            }
            else // Nếu không có coupon thì tính 2 dòng: dòng đầu tiên vat 10% của tonggiatour - tong doanh thu nn
            {
                var listct = _cthdvatRepository.ListCtVetourBySerial(tour, serial);
                if (tour == "OB") // Nếu tour OB thì tách làm 2 dòng, dòng 1 tính 10% vat trên tổng tổngtiền - tổng doanhthunn; dòng 2 vat 0% trên tổng doanhthu nước ngoài
                {
                    //decimal number1 = 0;
                    //decimal number2 = 0;
                    //var adl = listct.Where(x => x.serial == serial).Where(x => x.dotuoi == "ADL").Select(x => x.sotiennt);
                    //var doanhthunnCHD = listct.Where(x => x.serial == serial).Where(x => x.dotuoi != "ADL").Select(x => x.sotiennt);
                    //var doanhthunnADL = listct.Where(x => x.serial == serial).Where(x => x.dotuoi.Equals("ADL")).Select(x => x.doanhthunn);
                    //number1 = adl.Sum();
                    //number2 = doanhthunnADL.Sum();
                    //var star1 = number1 - number2;
                    //var star2 = number2 + doanhthunnCHD.Sum();
                    // Nếu vé tour mà > doanh thu nướci ngoài thì chia làm 2 dong// ngược lại thì chỉ lấy 1 dòng vé tour vat  0%
                    if(vetour.sotiennt>(vetour.doanhthunn * tygia))
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            cthdvat ct = new cthdvat();
                            ct.Idhoadon = Idhoadon;
                            ct.serial = vetour.serial;
                            ct.diengiai = vetour.diengiai;
                            ct.xuatve = Convert.ToDateTime(vetour.xuatve);
                            ct.tenkhach = vetour.tenkhach;
                            ct.sgtcode = vetour.sgtcode;
                            if (j == 0)
                            {
                                //ct.sotiennt = star1;
                                ct.diengiai = vetour.diengiai;
                                //ct.sotien = star1;
                                //ct.vat = 10;
                                ct.sotien = vetour.sotiennt - (vetour.doanhthunn * tygia);
                                ct.sotiennt = vetour.sotiennt - (vetour.doanhthunn * tygia);
                                ct.vat = 10;
                            }
                            else
                            {
                                //ct.sotiennt = star2;
                                //ct.sotien = star2;
                                ct.sotiennt = vetour.doanhthunn * tygia;
                                ct.sotien = vetour.doanhthunn * tygia;
                                ct.diengiai = vetour.diengiai;
                                ct.vat = 0;
                            }
                            ct.ppv = Convert.ToDecimal(ppv);
                            ct.tygia = (decimal)tygia;
                            ct.ghichu = vetour.ghichu;
                            ct.tkco = tkco;
                            ct.tkno = tkno;
                            ct.sttdong = 0;
                            ct.ngaytao = System.DateTime.Now;
                            ct.khachhuy = false;
                            ct.httc = "";
                            ct.dichvu = "";
                            ct.loaitien = vetour.loaitien;
                            ct.tour = tour;
                            ct.sk = vetour.sk;
                            newList.Add(ct);
                        }

                    }
                    // vé tour nhỏ hơn doanh thu nước ngoài, thì là  chính nó với vat =0%
                    else
                    {
                        cthdvat ct = new cthdvat();
                        ct.Idhoadon = Idhoadon;
                        ct.serial = vetour.serial;
                        ct.diengiai = vetour.diengiai;
                        ct.xuatve = Convert.ToDateTime(vetour.xuatve);
                        ct.tenkhach = vetour.tenkhach;
                        ct.sgtcode = vetour.sgtcode;
                        ct.sotien = vetour.sotiennt;
                        ct.sotiennt = vetour.sotiennt;
                        ct.vat = 0;
                        ct.diengiai = vetour.diengiai;
                        ct.ppv = Convert.ToDecimal(ppv);
                        ct.tygia = (decimal)tygia;
                        ct.ghichu = vetour.ghichu;
                        ct.tkco = tkco;
                        ct.tkno = tkno;
                        ct.sttdong = 0;
                        ct.ngaytao = System.DateTime.Now;
                        ct.khachhuy = false;
                        ct.httc = "";
                        ct.dichvu = "";
                        ct.loaitien = vetour.loaitien;
                        ct.tour = tour;
                        ct.sk = vetour.sk;
                        newList.Add(ct);
                    }
                   
                }
                else // Nếu là tour nội địa thì tính vat 10% trên từng vé tour
                {
                    cthdvat ct = new cthdvat();
                    ct.Idhoadon = Idhoadon;
                    ct.serial = vetour.serial;
                    ct.diengiai = vetour.diengiai;
                    ct.xuatve = Convert.ToDateTime(vetour.xuatve);
                    ct.tenkhach = vetour.tenkhach;
                    ct.sgtcode = vetour.sgtcode;
                    ct.sotien = vetour.sotiennt;// - tccoupon.Coupon;
                    ct.sotiennt = vetour.sotiennt;// - tccoupon.Coupon;
                    ViewBag.tongtien = vetour.sotiennt;// - tccoupon.Coupon;
                    ct.vat = 10;
                    ct.ppv = Convert.ToDecimal(ppv);
                    ct.tygia = (decimal)tygia;
                    ct.ghichu = vetour.ghichu;
                    ct.tkco = tkco;
                    ct.tkno = tkno;
                    ct.sttdong = 0;
                    ct.ngaytao = System.DateTime.Now;
                    ct.khachhuy = false;
                    ct.httc = "";
                    ct.dichvu = "";
                    ct.loaitien = vetour.loaitien;
                    ct.tour = tour;
                    ct.sk = vetour.sk;
                    newList.Add(ct);
                }
            }
            return View(newList);
        }
        #endregion
        #region Xoá chi tiết hoá đơn
        
        [HttpPost]
        public ActionResult Xoacthd(decimal id)
        {
            var cthd = _cthdvatRepository.GetById(id);
            if (string.IsNullOrEmpty(cthd.serial))
            {
                cthd.logfile += cthd.logfile + System.Environment.NewLine + "=========================" + System.Environment.NewLine + "User " + HttpContext.Session.GetString("username") + " xoá chi tiết hoá đơn lúc " + System.DateTime.Now;
                _cthdvatRepository.Update(cthd);
                _cthdvatRepository.Delete(cthd);
            }
            else
            {
                var lcthd = _cthdvatRepository.GetAll().Where(x => x.chinhanh == cthd.chinhanh && x.serial == cthd.serial).ToList();
                foreach( var c in lcthd)
                {
                    var i = _cthdvatRepository.GetById(c.Id);
                    i.logfile += i.logfile + System.Environment.NewLine + "=========================" + System.Environment.NewLine + "User " + HttpContext.Session.GetString("username") + " xoá chi tiết hoá đơn lúc " + System.DateTime.Now;
                    _cthdvatRepository.Update(i);
                    _cthdvatRepository.Delete(i);
                }
            }
           // return RedirectToAction("Edit", "Hoadon", new { id = cthd.Idhoadon });
             return Json(true);
        }

        #endregion
        #region Cập nhật chi tiết hoá đơn
        [HttpGet]
        public ActionResult Capnhatcthd(decimal id)
        {
            var cthd = _cthdvatRepository.GetById(id);
            //  var hd = _hoadonRepository.Find( x=>x.Idhoadon== cthd.Idhoadon && x.chinhanh== HttpContext.Session.GetString("username")).FirstOrDefault();
            var hd = _hoadonRepository.GetByTwoKey(cthd.Idhoadon, HttpContext.Session.GetString("chinhanh"));
            ViewBag.idhoadon = cthd.Idhoadon;

            ListNgoaite(cthd.loaitien);
            ListNgoaite(cthd.loaitien);
            Listhttc(cthd.httc);
            Tkco(cthd.tkco);
            Tkno(cthd.tkno);
            ViewBag.hide = "Cập nhật";
            // var user = HttpContext.Session.GetString("username");
            //if (hd.nguoitaohd != HttpContext.Session.GetString("username"))
            //{
            //    ViewBag.hide = "hide";
            //}
            if (!String.IsNullOrEmpty(hd.keyhddt))
            {
                SetAlert("Đây là hoá đơn điện tử, không được điều chỉnh thông tin khi đã xuất hoá đơn", "error");
                return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
            }
            return View("capnhatcthd", cthd);
        }

        [HttpPost]
        public ActionResult Capnhatcthd(cthdvat entity)
        {
            temp = ""; log = "";

            cthdvat ct = _cthdvatRepository.GetById(entity.Id);

            ct.sttdong = string.IsNullOrEmpty(ct.sttdong.ToString()) ? 0 : ct.sttdong;

            if (ct.sttdong != entity.sttdong)
            {
                temp += string.Format("- STT thay đổi: {0}->{1}", ct.sttdong, entity.sttdong);
            }
            if (ct.diengiai != entity.diengiai)
            {
                temp += string.Format("- Diễn giải thay đổi: {0}->{1}", ct.diengiai, entity.diengiai);
            }
            if (ct.tenkhach != entity.tenkhach)
            {
                temp += string.Format("- Tên khách thay đổi: {0}->{1}", ct.tenkhach, entity.tenkhach);
            }

            if (ct.sgtcode != entity.sgtcode)
            {
                temp += string.Format("- Tour code thay đổi: {0}->{1}", ct.sgtcode, entity.sgtcode);
            }
            if (ct.sk != entity.sk)
            {
                temp += string.Format("- Số khách thay đổi: {0}->{1}", ct.sk, entity.sk);
            }
            if (ct.sotiennt != entity.sotiennt)
            {
                temp += string.Format("- Tiền NT thay đổi: {0:#,##0.0}->{1:#,##0.0}", ct.sotiennt, entity.sotiennt);
            }
            if (ct.loaitien != entity.loaitien)
            {
                temp += string.Format("- Loại tiền thay đổi thay đổi: {0}->{1}", ct.loaitien, entity.loaitien);
            }
            if (ct.tygia != entity.tygia)
            {
                temp += string.Format("- Tỷ giá thay đổi: {0}->{1}", ct.tygia, entity.tygia);
            }
            if (ct.sotien != entity.sotien)
            {
                temp += string.Format("- Tiền VNĐ thay đổi: {0:#,##0}->{1:#,##0}", ct.sotien, entity.sotien);
            }
            if (ct.ppv != entity.ppv)
            {
                temp += string.Format("- Phí phục vụ thay đổi: {0}->{1}", ct.ppv, entity.ppv);
            }
            if (ct.vat != entity.vat)
            {
                temp += string.Format("- VAT thay đổi: {0}->{1}", ct.vat, entity.vat);
            }
            var httc = string.IsNullOrEmpty(entity.httc) ? "" : entity.httc;
            if (ct.httc != httc)
            {
                temp += string.Format("- HTTC thay đổi: {0}->{1}", ct.httc, entity.httc);
            }
            if (ct.tkno != entity.tkno)
            {
                temp += string.Format("- Tài khoản nợ thay đổi: {0}->{1}", ct.tkno, entity.tkno);
            }
            if (ct.tkco != entity.tkco)
            {
                temp += string.Format("- Tài khoản có thay đổi: {0}->{1}", ct.tkco, entity.tkco);
            }
            if (ct.ghichu != entity.ghichu)
            {
                temp += string.Format("- Ghi chú thay đổi: {0}->{1}", ct.ghichu, entity.ghichu);
            }
            ct.sttdong = entity.sttdong;
            ct.diengiai = entity.diengiai;
            ct.tenkhach = entity.tenkhach;
            ct.sgtcode = entity.sgtcode;
            ct.sotiennt = entity.sotiennt;
            ct.loaitien = entity.loaitien;
            ct.tygia = entity.tygia;
            ct.sotien = entity.sotien;
            ct.ppv = entity.ppv;
            ct.vat = entity.vat;
            ct.httc = string.IsNullOrEmpty(entity.httc) ? "" : entity.httc;
            ct.tkno = entity.tkno;
            ct.tkco = entity.tkco;
            ct.ghichu = entity.ghichu;
            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User cập nhật chi tiết hoá đơn: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ct.logfile = ct.logfile + log;
            }
            var result = _cthdvatRepository.Update(ct);
            if (result != null)
            {
                SetAlert("Cập nhật chi tiết hoá đơn thành công", "success");
            }
            else
            {
                SetAlert("Cập nhật chi tiết hoá đơn không thành công", "error");
            }

            return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
        }

        [HttpPost]
        public ActionResult updateSgtcode(decimal id, string sgtcode)
        {
            var cthd = _cthdvatRepository.GetById(id);
            
            temp = ""; log= "";
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
            var result = _cthdvatRepository.Update(cthd);
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
        #region Thêm chi tiết hoá đơn
        public ActionResult Themcthd(string idhoadon)
        {
            ViewBag.id = idhoadon;
            var hd = _hoadonRepository.GetByTwoKey(idhoadon, HttpContext.Session.GetString("chinhanh"));
            var ct = new cthdvat();
            ct.Idhoadon = idhoadon;
            ct.sttdong = 0;
            ct.tygia = 1;
            ListNgoaite("VND");
            ListHttt("TM");
            Listhttc("");
            if (hd.coupon)
            {
                Tkco("3387");
            }
            else
            {
                Tkco("5113333335");
            }
            Tkno("1311110000");
            // var hd = _hoadonRepository.Find(x=>x.Idhoadon==idhoadon && x.chinhanh== HttpContext.Session.GetString("chinhanh")).FirstOrDefault();
            
            
            if (!String.IsNullOrEmpty(hd.keyhddt))
            {
                SetAlert("Đây là hoá đơn điện tử, không được điều chỉnh thông tin khi đã xuất hoá đơn", "error");
                return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
            }

            return View(ct);
        }
        [HttpPost]
        public ActionResult Themcthd(cthdvat entity)
        {
            entity.ngaytao = System.DateTime.Now;
            entity.tour = "";
            entity.httc = entity.httc ?? "";
            entity.diengiai = string.IsNullOrEmpty(entity.diengiai) ? "" : entity.diengiai;
            entity.sgtcode = string.IsNullOrEmpty(entity.sgtcode) ? "" : entity.sgtcode;
            entity.tenkhach = string.IsNullOrEmpty(entity.tenkhach) ? "" : entity.tenkhach;
            entity.chinhanh = HttpContext.Session.GetString("chinhanh");
            entity.logfile = "-User tạo chi tiết hoá đơn: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString();
            var result = _cthdvatRepository.Create(entity);
            if (result != null)
            {
                SetAlert("Thêm chi tiết hoá đơn thành công", "success");
            }
            else
            {
                SetAlert("Thêm chi tiết hoá đơn không thành công", "error");
            }
            return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
        }
        #endregion

        #region Thêm chi tiết từ huỷ hoá đơn
        public ActionResult ThemctTuhuyhd(string idhoadon, string stt)
        {
            ViewBag.stt = stt;
            ViewBag.idhoadon = idhoadon;
            var hd = _hoadonRepository.getHoadonbyStt(stt);
            if (hd == null)
            {
                return View("ThemctTuhuyhd");
            }
            var ct = _cthdvatRepository.ListChitietHoadonhuy(hd.Idhoadon, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = ct.Count();

            return View(ct);
        }
        [HttpPost]
        public ActionResult ThemctTuhuyhd_(string idhoadon, string listString)
        {
            var cthd = JsonConvert.DeserializeObject<List<cthdvat>>(listString);
            foreach (var c in cthd)
            {
                var ct = _cthdvatRepository.GetById(c.Id);
                cthdvat h = new cthdvat();
                //ct.Idhoadon = idhoadon;
                h.chinhanh = HttpContext.Session.GetString("chinhanh");
                h.Idhoadon = idhoadon;
                h.diengiai = c.diengiai;
                h.sotien = c.sotien;
                h.serial = ct.serial;
                h.xuatve = ct.xuatve;
                h.tenkhach = ct.tenkhach;
                h.sgtcode = ct.sgtcode;
                h.sotiennt = ct.sotiennt;
                h.sotien = ct.sotien;
                h.loaitien = ct.loaitien;
                h.tygia = ct.tygia;
                h.ppv = ct.ppv;
                h.vat = ct.vat;
                h.ghichu = ct.ghichu;
                h.coupon = ct.coupon;
                h.tiencoupon = ct.tiencoupon;
                h.tour = ct.tour;
                h.httc = ct.httc;
                h.tkno = ct.tkno;
                h.tkco = ct.tkco;
                h.ngaytao = System.DateTime.Now;
                h.logfile = "User thêm chi tiết từ hoá đơn " + c.hoadonhuy + " đã huỷ: " + HttpContext.Session.GetString("username") + " vào lúc " + System.DateTime.Now;
                // _huycthdvatRepository.Create(h);
                _cthdvatRepository.Create(h);
            }

            return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
        }

        #endregion
        public ActionResult ViewlogCthoadon(decimal id)
        {
            var cthd = _cthdvatRepository.GetById(id);
            return PartialView("ViewlogCthoadon", cthd);
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
                                                     where s.tkhoan.StartsWith("511") || s.tkhoan.StartsWith("3331") || s.tkhoan.StartsWith("3387")
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
