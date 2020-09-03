using System;
using System.Collections.Generic;
using System.Linq;
using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace hdvatob.Controllers
{
    public class HoadonController : BaseController
    {
        private readonly IHoadonRepository _hoadonRepository;
        private readonly ICthdvatRepository _cthdvatRepository;
        private readonly IDsdangkyhdRepository _dsdangkyhdRepository;
        private readonly IDmtkRepository _dmtkRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ITachveRepository _tachveRepository;
        private readonly ICttachveRepository _cttachveRepository;
        private readonly INguonhdRepository _nguonhdRepository;
        string temp, log = "";//,_username, _chinhanh, _tour, _kyhieu, _maviettat,_mausohd = "";

        //private PortalServiceSoapClient portalService;
        //private readonly PublishServiceSoapClient publishService;
        //private readonly BusinessServiceSoapClient businessService;
        public HoadonController(IHoadonRepository hoadonRepository, ICthdvatRepository cthdvatRepository,
                                IDsdangkyhdRepository dsdangkyhdRepository, IDmtkRepository dmtkRepository,
                                ISupplierRepository supplierRepository, ITachveRepository tachveRepository,
                                ICttachveRepository cttachveRepository, INguonhdRepository nguonhdRepository)
        {
            _hoadonRepository = hoadonRepository;
            _cthdvatRepository = cthdvatRepository;
            _dsdangkyhdRepository = dsdangkyhdRepository;
            _dmtkRepository = dmtkRepository;
            _supplierRepository = supplierRepository;
            _tachveRepository = tachveRepository;
            _cttachveRepository = cttachveRepository;
            _nguonhdRepository = nguonhdRepository;
        
        }
        public IActionResult Index(string searchString, int page = 1)
        {
            searchString = searchString ?? "";
            //chinhanh = HttpContext.Session.GetString("chinhanh");// HttpContext.Session.GetString("chinhanh");
            HttpContext.Session.SetString("urlHoadon", UriHelper.GetDisplayUrl(Request));
            var hoadon = _hoadonRepository.ListHoadon(searchString, HttpContext.Session.GetString("chinhanh"), page);
            ViewData["CurrentFilter"] = searchString;
            ViewBag.hoadon = hoadon;
            ViewBag.username = HttpContext.Session.GetString("username");
            return View(hoadon);
        }

        #region Tạo hoá đơn
        public ActionResult Create()
        {
            var hd = new Hoadon();
            hd.ngayct = System.DateTime.Now;
            hd.kyhieu = HttpContext.Session.GetString("kyhieuhd").Trim();
            hd.mausohd = HttpContext.Session.GetString("mausohd").Trim();
            // hd.stt = tthoadon.maviettat; _hoadonRepository.newStt(tthoadon.maviettat);
            listHttt("TM/CK");
            return View(hd);
        }

        [HttpPost]
        public ActionResult Create(Hoadon entity)
        {
            //string maviettat = entity.stt;
            entity.Idhoadon = _hoadonRepository.newId(HttpContext.Session.GetString("chinhanh"));
            //entity.keyhddt = entity.keyhddt.Trim();
            //entity.mausohd = entity.mausohd.Trim();
            entity.ngaytao = System.DateTime.Now;
            string firstId = entity.Idhoadon.Substring(0, 6);
            string last = entity.Idhoadon.Substring(6, 4);
            entity.stt = firstId + HttpContext.Session.GetString("maviettat") + last;
            entity.chinhanh = HttpContext.Session.GetString("chinhanh");
            entity.user = HttpContext.Session.GetString("username");
            entity.nguoitaohd = HttpContext.Session.GetString("username");
            entity.logfile = " User tạo hoá đơn :" + HttpContext.Session.GetString("username") + " vào lúc " + System.DateTime.Now.ToString();
            var result = _hoadonRepository.Create(entity);
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

        #region Cập nhật hoá đơn
        public IActionResult Edit(string id)
        {
            TempData["idhoadon"] = id;
            HttpContext.Session.SetString("urlEditHoadon", UriHelper.GetDisplayUrl(Request));
            if (id == null)
            {
                return NotFound();
            }
            //var hoadon = _hoadonRepository.Find(x => x.Idhoadon == id && x.chinhanh == HttpContext.Session.GetString("chinhanh")).FirstOrDefault();
            var hoadon = _hoadonRepository.GetByTwoKey(id, HttpContext.Session.GetString("chinhanh"));
            if (hoadon == null)
            {
                return NotFound();
            }
            ViewBag.idhoadon = id;
            listHttt(hoadon.httt);
            if (string.IsNullOrEmpty(hoadon.keyhddt))// && hoadon.nguoitaohd == HttpContext.Session.GetString("username"))
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
        public ActionResult Edit(Hoadon entity)
        {
            temp = ""; log = "";
            var hd = _hoadonRepository.GetByTwoKey(entity.Idhoadon, HttpContext.Session.GetString("chinhanh"));
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
            if (hd.batdau != entity.batdau)
            {
                temp += String.Format("- Ngày bắt đầu thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", hd.batdau, entity.batdau);
            }
            if (hd.ketthuc != entity.ketthuc)
            {
                temp += String.Format("- Ngày kết thúc thay đổi: {0:dd/MM/yyyy}->{1:dd/MM/yyyy}", hd.ketthuc, entity.ketthuc);
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
            hd.batdau = entity.batdau;
            hd.ketthuc = entity.ketthuc;
            hd.logfile = hd.logfile ?? "";

            if (temp.Length > 0)
            {
                log = System.Environment.NewLine;
                log += "=============";
                log += System.Environment.NewLine;
                log += temp + " -User cập nhật hoá đơn vat: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                hd.logfile = hd.logfile + log;
            }
            var result = _hoadonRepository.Update(hd);
            if (result != null)
            {
                SetAlert("Cập nhật hoá đơn thành công", "success");

            }
            else
            {
                SetAlert("Cập nhật hoá đơn không thành công", "error");
            }
            return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
        }
        
        #endregion

        #region huỷ hoá đơn
        [HttpPost]
        public ActionResult Delete(string idhoadon)
        {
            var hd = _hoadonRepository.GetByTwoKey(idhoadon,HttpContext.Session.GetString("chinhanh"));
            hd.ngayxoa = System.DateTime.Now;
            hd.logfile = hd.logfile ?? "";
            hd.logfile = hd.logfile + System.Environment.NewLine + "=========================" + System.Environment.NewLine + "User " + HttpContext.Session.GetString("username") + " xoá hoá đơn lúc " + System.DateTime.Now;
            _hoadonRepository.Update(hd);
            _hoadonRepository.Delete(hd);

            List<cthdvat> cthd = _cthdvatRepository.ListChitietHoadon(hd.Idhoadon, HttpContext.Session.GetString("chinhanh")).ToList();
            foreach (cthdvat c in cthd)
            {
                c.logfile = c.logfile ?? "";
                c.logfile = c.logfile + System.Environment.NewLine + "======================" + System.Environment.NewLine + " User: " + HttpContext.Session.GetString("username") + " xoá hoá đơn " + c.Idhoadon + " , xoá luôn các chi tiết, lúc: " + System.DateTime.Now;
                _cthdvatRepository.Update(c);
                _cthdvatRepository.Delete(c);
            }
            return Json(true);
        }

        #endregion

        #region Data Từ Vé tour
        [HttpGet]
        public ActionResult Datatuvetour(string idhoadon, string tour, decimal ppv, decimal tygia, string tuyentq, string tungay, string denngay,string tkno,string tkco)
        {
            //var hd = _hoadonRepository.GetByTwoKey(x=>x.Idhoadon==idhoadon && x.chinhanh== HttpContext.Session.GetString("chinhanh")).FirstOrDefault();
            var hd = _hoadonRepository.GetByTwoKey(idhoadon, HttpContext.Session.GetString("chinhanh"));
            //tour = tour ?? "OB";
            if (HttpContext.Session.GetString("chinhanh") == "STN")
            {
                tour = "ND";
            }
            tour = tour ?? "OB";
            ViewBag.tour = tour;
            tuyentq = tuyentq ?? "";
            tungay = tungay ?? hd.ngayct.Value.ToString("dd/MM/yyyy");
            denngay = denngay ?? hd.ngayct.Value.ToString("dd/MM/yyyy");
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            tkno = tkno ?? "1311110000";
            tkco = tkco ?? "5113333335";
            ViewBag.tkno = tkno;
            ViewBag.tkco = tkco;
            listNguontour(ViewBag.tour);
            Tkco(ViewBag.tkco);
            Tkno(ViewBag.tkno);

            ViewBag.chinhanh = HttpContext.Session.GetString("chinhanh");
            ViewBag.idhoadon = idhoadon;
            ViewBag.tuyentq = tuyentq;
            ViewBag.ppv = string.IsNullOrEmpty(ppv.ToString()) ? 0 : ppv;
            tygia = tygia==0 ? 1 : tygia;
            ViewBag.tygia = tygia;
            ViewBag.tuyentq = tuyentq;
            if (string.IsNullOrEmpty(tour))
            {
                return View("Datatuvetour");
            }
            
            var d = _hoadonRepository.listdatavetour(tour, tungay, denngay, tuyentq, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d.Count == 0)
            {
                return View("Datatuvetour");
            }
            else
            {
                return View("Datatuvetour", d);
            }
        }
        [HttpPost]
        public ActionResult Datatuvetour(string idhoadon, decimal ppv, decimal tygia, string tour, string tkno, string tkco, string list)
        {           
            tygia = tygia == 0 ? 1 : tygia;
            ppv = string.IsNullOrEmpty(ppv.ToString()) ? 0 : ppv;
            var idList = JsonConvert.DeserializeObject<List<DataTuVetourViewModel>>(list);
            // chỉ lấy danh sách serial của các vetour
            var listSerial = idList.Select(x => x.serial).Distinct();

            foreach (var seri in listSerial)
            {
                // Lấy thông tin của từng vé tour 
                var i = _hoadonRepository.GetVetourBySerial(tour, seri, HttpContext.Session.GetString("chinhanh"));
                var tccoupon = _hoadonRepository.GetTienCoupon(tour, i.id);
                if (tccoupon != null) // Nếu tiền coupon > 0 
                {
                    cthdvat ct = new cthdvat();
                    ct.Idhoadon = idhoadon;
                    ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                    ct.serial = i.serial;
                    ct.xuatve = Convert.ToDateTime(i.xuatve);
                    ct.tenkhach = i.tenkhach;
                    ct.sgtcode = i.sgtcode;
                    if (tour == "OB")
                    {
                        if (i.sotiennt - tccoupon.Coupon > (i.doanhthunn * tygia))
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (j == 0)
                                {
                                    ct.sotien = i.sotiennt - tccoupon.Coupon - (i.doanhthunn * tygia);
                                    ct.sotiennt = i.sotiennt - tccoupon.Coupon - (i.doanhthunn * tygia);
                                    ct.vat = 10;
                                }
                                else
                                {
                                    ct.sotien = i.sotiennt - tccoupon.Coupon;
                                    ct.sotien = i.sotiennt - tccoupon.Coupon;
                                    ct.vat = 0;
                                }
                                ct.ppv = Convert.ToDecimal(ppv);
                                ct.tygia = (decimal)tygia;
                                ct.ghichu = i.ghichu;
                                ct.diengiai = i.diengiai;
                                ct.tkco = tkco;
                                ct.tkno = tkno;
                                ct.sttdong = 0;
                                ct.ngaytao = System.DateTime.Now;
                                ct.khachhuy = false;
                                ct.httc = "";
                                ct.dichvu = "";
                                ct.loaitien = i.loaitien;
                                ct.tour = tour;
                                ct.sk = i.sk;
                                ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                                ct.logfile = " -User tạo chi tiết hoá đơn từ datatour: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                _cthdvatRepository.Create(ct);
                            }
                        }
                        else
                        {
                            ct.sotien = i.sotiennt - tccoupon.Coupon;
                            ct.sotien = i.sotiennt - tccoupon.Coupon;
                            ct.vat = 0;
                            ct.diengiai = i.diengiai;
                            ct.ppv = Convert.ToDecimal(ppv);
                            ct.tygia = (decimal)tygia;
                            ct.ghichu = i.ghichu;
                            ct.tkco = tkco;
                            ct.tkno = tkno;
                            ct.sttdong = 0;
                            ct.ngaytao = System.DateTime.Now;
                            ct.khachhuy = false;
                            ct.httc = "";
                            ct.dichvu = "";
                            ct.loaitien = i.loaitien;
                            ct.tour = tour;                           
                            ct.sk = i.sk;
                            ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                            ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                            ct.logfile = " -User tạo chi tiết hoá đơn từ datatour: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            _cthdvatRepository.Create(ct);
                        }
                    }
                    //ct.logfile = " -User tạo chi tiết hoá đơn từ datatour: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    //_cthdvatRepository.Create(ct);
                    else
                    {
                        ct.sotien = i.sotiennt - tccoupon.Coupon;
                        ct.sotiennt = i.sotiennt - tccoupon.Coupon;
                        ct.vat = 10;
                        ct.ppv = Convert.ToDecimal(ppv);
                        ct.tygia = (decimal)tygia;
                        ct.ghichu = i.ghichu;
                        ct.diengiai = i.diengiai;
                        ct.tkco = tkco;
                        ct.tkno = tkno;
                        ct.sttdong = 0;
                        ct.ngaytao = System.DateTime.Now;
                        ct.khachhuy = false;
                        ct.httc = "";
                        ct.dichvu = "";
                        ct.loaitien = i.loaitien;
                        ct.tour = tour;
                        ct.sk = i.sk;
                        ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                        ct.logfile = " -User tạo chi tiết hoá đơn từ datatour: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        _cthdvatRepository.Create(ct);
                    }
                    // cap nhat lai hoa don set coupon =1
                    //Hoadon hd = _hoadonRepository.GetByTwoKey(idhoadon, HttpContext.Session.GetString("chinhanh"));
                    //hd.coupon = true;
                    //_hoadonRepository.Update(hd);
                    
                }
                else // nếu không có tiền coupon
                {

                    // Lấy danh sách chi tiết của từng vé tour (danh sách0 khách)
                    var c = _cthdvatRepository.ListCtVetourBySerial(tour, seri);

                    ////////////////////////////////////////////////////////////////////////////////////////

                    if (tour == "OB")
                    {
                        //decimal number1 = 0;
                        //decimal number2 = 0;
                        //var adl = c.Where(x => x.serial == seri).Where(x => x.dotuoi.Equals("ADL")).Select(x => x.sotiennt);
                        //var doanhthunnCHD = c.Where(x => x.serial == seri).Where(x => x.dotuoi != "ADL").Select(x => x.sotiennt);
                        //var doanhthunnADL = c.Where(x => x.serial == seri).Where(x => x.dotuoi.Equals("ADL")).Select(x => x.doanhthunn);
                        //number1 = adl.Sum();
                        //number2 = doanhthunnADL.Sum();
                        //var star1 = number1 - number2;
                        //var star2 = number2 + doanhthunnCHD.Sum();

                        // Nếu tiền vé tour > tổng doanh thu nước ngoài, thì chia là 2 dòng
                        if (i.sotiennt > (i.doanhthunn * tygia))
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                cthdvat ct = new cthdvat();
                                ct.Idhoadon = idhoadon;
                                ct.serial = i.serial;
                                ct.diengiai = i.diengiai;
                                ct.xuatve = Convert.ToDateTime(i.xuatve);
                                ct.tenkhach = i.tenkhach;
                                ct.sgtcode = i.sgtcode;
                                if (j == 0)
                                {
                                    //ct.sotiennt = star1;
                                    ct.diengiai = i.diengiai;
                                    //ct.sotien = star1;
                                    //ct.vat = 10;
                                    ct.sotien = i.sotiennt - (i.doanhthunn * tygia);
                                    ct.sotiennt = i.sotiennt - (i.doanhthunn * tygia);
                                    ct.vat = 10;
                                }
                                else
                                {
                                    //ct.sotiennt = star2;
                                    //ct.sotien = star2;
                                    ct.sotiennt = i.doanhthunn * tygia;
                                    ct.sotien = i.doanhthunn * tygia;
                                    ct.diengiai = i.diengiai;
                                    ct.vat = 0;
                                }
                                ct.ppv = Convert.ToDecimal(ppv);
                                ct.tygia = (decimal)tygia;
                                ct.ghichu = i.ghichu;
                                ct.tkco = tkco;
                                ct.tkno = tkno;
                                ct.sttdong = 0;
                                ct.ngaytao = System.DateTime.Now;
                                ct.khachhuy = false;
                                ct.httc = "";
                                ct.dichvu = "";
                                ct.loaitien = i.loaitien;
                                ct.tour = tour;
                                ct.sk = i.sk;
                                ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                                ct.logfile = " -User tạo chi tiết hoá đơn từ datatour: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                _cthdvatRepository.Create(ct);
                                //if (ct.sotien > 0)
                                //{
                                //    // Nếu số tiền >0 thì mới thêm vô hoá đơn - Đây là trường hợp khách đi tour, ngược lại khách chỉ mua dịch vụ không tính thuế VAT
                                //    _cthdvatRepository.Create(ct);
                                //}
                            }
                        }
                        // số tiền vé tour <= tổng doanh thu nước ngoài thì số tiền bằng chính nó
                        else
                        {
                            cthdvat ct = new cthdvat();
                            ct.Idhoadon = idhoadon;
                            ct.serial = i.serial;
                            ct.diengiai = i.diengiai;
                            ct.xuatve = Convert.ToDateTime(i.xuatve);
                            ct.tenkhach = i.tenkhach;
                            ct.sgtcode = i.sgtcode;
                            ct.sotien = i.sotiennt;
                            ct.sotiennt = i.sotiennt;
                            ct.vat = 0;
                            ct.ppv = Convert.ToDecimal(ppv);
                            ct.tygia = (decimal)tygia;
                            ct.ghichu = i.ghichu;
                            ct.tkco = tkco;
                            ct.tkno = tkno;
                            ct.sttdong = 0;
                            ct.ngaytao = System.DateTime.Now;
                            ct.khachhuy = false;
                            ct.httc = "";
                            ct.dichvu = "";
                            ct.loaitien = i.loaitien;
                            ct.tour = tour;
                            ct.sk = i.sk;
                            ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                            ct.logfile = " -User tạo chi tiết hoá đơn từ datatour: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            _cthdvatRepository.Create(ct);
                        }
                    }
                    else
                    {
                        cthdvat ct = new cthdvat();
                        ct.Idhoadon = idhoadon;
                        ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                        ct.serial = i.serial;
                        ct.diengiai = i.serial +" "+i.diengiai;
                        ct.xuatve = Convert.ToDateTime(i.xuatve);
                        ct.tenkhach = i.tenkhach;
                        ct.sgtcode = i.sgtcode;
                        ct.sotien = i.sotiennt;// - tccoupon.Coupon;
                        ct.sotiennt = i.sotiennt;// - tccoupon.Coupon;
                        ct.vat = 10;
                        ct.ppv = Convert.ToDecimal(ppv);
                        ct.tygia = (decimal)tygia;
                        ct.ghichu = i.ghichu;
                        ct.tkco = tkco;
                        ct.tkno = tkno;
                        ct.sttdong = 0;
                        ct.ngaytao = System.DateTime.Now;
                        ct.khachhuy = false;
                        ct.httc = "";
                        ct.dichvu = "";
                        ct.loaitien = "VND";
                        ct.tour = tour;
                        ct.sk = i.sk;
                        ct.coupon = 0;
                        ct.tiencoupon = 0;
                        ct.logfile = " -User tạo chi tiết hoá đơn từ datatour: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        _cthdvatRepository.Create(ct);
                    }
                }
            }

            return RedirectToAction("Edit", new { id = idhoadon });
        }
        #endregion

        #region Tách vé tour thành nhiều hoá đơn
        [HttpGet]
        public ActionResult Tachvetour(string serial, int slhoadon, decimal ppv, decimal tygia, string makh, string tenkh, string tkno, string tkco)
        {            
            tygia = tygia == 0 ? 1 : tygia;
            slhoadon = slhoadon == 0 ? 2 : slhoadon;
            ppv = string.IsNullOrEmpty(ppv.ToString()) ? 0 : ppv;
            Tkco("5113333336");
            Tkno("1311110000");
            ViewBag.serial = serial;
            ViewBag.slhoadon = slhoadon;
            ViewBag.tygia = tygia;
            ViewBag.ppv = ppv;
            ViewBag.makh = makh;
            ViewBag.tenkh = tenkh;
            //ViewBag.tkno = tkno;
            //ViewBag.tkco = tkco;
            if (string.IsNullOrEmpty(serial))
            {
                return View("Tachvetour");
            }
            var d = _tachveRepository.TachvetourBySerial(serial, HttpContext.Session.GetString("chinhanh"));
            if (d == null)
            {
                SetAlert("Số vé tour này không tồn tại, hoặc không do "+HttpContext.Session.GetString("chinhanh")+" bán.", "error");
                return View("Tachvetour");
            }

            if (d.chon == 0)
            {
                SetAlert("Số vé tour " + serial + " đã xuất hoá đơn, xin chọn số vé tour khách", "error");
                return View("Tachvetour");
            }
            if (d.chon == 1)
            {
                SetAlert("Số vé tour " + serial + " đã tách rồi, xin chọn số vé tour khách", "error");
                return View("Tachvetour");
            }
            if(d.chon==1)
            {
                var tachve = _tachveRepository.Find(x => x.serial == d.serial).ToList();
                string xuatve = "";
                foreach (var i in tachve)
                {
                    if (xuatve == "")
                    {
                        xuatve = string.Format("{0:dd/MM/yyyy}", i.ngayct);
                    }
                    else
                    {
                        xuatve += "-" + string.Format("{0:dd/MM/yyyy}", i.ngayct);
                    }

                }
                SetAlert("Số vé tour " + serial + " đã tách thành nhiều hoá đơn ngày " + xuatve + ", xin chọn số vé tour khách", "error");
                return View("Tachvetour");
            }
            
            supplier s = _supplierRepository.GetByTwoKey(makh,HttpContext.Session.GetString("chinhanh"));
            if (s == null)
            {
                SetAlert("Khách hàng không tồn tại.", "error");
                return View("Tachvetour");
            }
            //string vetour = serial.Substring(6, 2);
            string tour = "";
            switch (serial.Substring(6, 2))
            {
                case "TO":
                    tour = "OB";
                    break;
                case "TN":
                    tour = "ND";
                    break;
                case "TW":
                    tour = "WI";
                    break;
                default:
                    break;
            }
            var tcoupon = _hoadonRepository.GetTienCoupon(tour, d.stt);
            decimal doanhthunn = d.doanhthunn * tygia, tientourcoupon = 0, tientach=0, doanhthutach=0 ,tongtach=0,tongdttach=0;
            if (tcoupon != null)
            {
                tientourcoupon = d.sotiennt - tcoupon.Coupon;
            }
            else
            {
                tientourcoupon = d.sotiennt ;
            }

            tientach = Math.Round((tientourcoupon / slhoadon),1);
            doanhthutach = Math.Round( (doanhthunn / slhoadon),1);
            DateTime dt = System.DateTime.Now;
            List<TachveViewModel> listhd = new List<TachveViewModel>();
            List<DataTuVetour> data = new List<DataTuVetour>();

            if (tour == "OB")
            {
                for (int i = 0; i < slhoadon; i++)
                {
                    var hd = new TachveViewModel();
                    hd.stt = i.ToString();
                    hd.ngayct = dt.ToString("dd/MM/yyyy");
                    hd.makh = string.IsNullOrEmpty(s.code) ? "" : s.code;
                    hd.tenkh = string.IsNullOrEmpty(s.name) ? "" : s.name;
                    hd.tenkhach = d.tenkhach;
                    hd.diachi = string.IsNullOrEmpty(s.address) ? "" : s.address;
                    hd.dienthoai = string.IsNullOrEmpty(s.telephone) ? "" : s.telephone;
                    hd.msthue = string.IsNullOrEmpty(s.taxcode) ? "" : s.taxcode;
                    hd.serial = d.serial;
                    hd.ghichu = d.ghichu;
                    hd.nguoitach = HttpContext.Session.GetString("username");
                    listhd.Add(hd);
                   
                    if (tientach > doanhthutach) // nếu tiền tour > doanh thu nước ngoài thì chia là 2 dòng
                    {
                        for(int j = 0; j < 2; j++)
                        {
                            var da = new DataTuVetour();
                            da.stt = i ;
                            da.diengiai = d.diengiai;
                            da.serial = d.serial;
                            da.xuatve = d.xuatve;
                            da.tenkhach = d.tenkhach;
                            da.sgtcode = d.sgtcode;
                            da.vat = 0;
                            da.tygia = tygia;
                            da.loaitien = d.loaitien;
                           // da.ghichu = d.ghichu;
                            if (j == 0)
                            {
                                da.sotiennt = tientach - doanhthutach;
                                da.vat = 10;
                            }
                            else
                            {
                                da.sotiennt = doanhthutach;
                                da.vat = 0;
                            }                            
                            da.ghichu= dt.ToString("dd/MM/yyyy"); ;
                            data.Add(da);
                        }
                    }
                    else // ngược lại chỉ lấy 1 dòng với số tiền là tiền tour với VAT 0%
                    {
                        var da = new DataTuVetour();
                        da.stt = i;
                        da.diengiai = d.diengiai;
                        da.serial = d.serial;
                        da.xuatve = d.xuatve;
                        da.tenkhach = d.tenkhach;
                        da.sgtcode = d.sgtcode;
                        da.vat = 0;
                        da.tygia = tygia;
                        da.loaitien = d.loaitien;
                        da.ghichu = dt.ToString("dd/MM/yyyy");
                        if (i < slhoadon - 1)
                        {
                            da.sotiennt = tientach;
                            da.doanhthunn = doanhthutach;
                            tongtach += tientach;
                            tongdttach += doanhthutach;
                        }
                        else
                        {
                            da.sotiennt = tientourcoupon - tongtach;
                            da.doanhthunn = doanhthunn - doanhthutach;
                        }
                      
                        data.Add(da);
                    }
                    dt = dt.AddDays(1);
                }
            }
            else
            {
                // các tour ND,WI,DH thì chỉ mỗi hoá đơn là 1 dòng với VAT 10%
                for (int i = 0; i < slhoadon; i++)
                {
                    var hd = new TachveViewModel();
                    hd.stt = i.ToString();
                    hd.ngayct = dt.ToString("dd/MM/yyyy");
                    hd.makh = string.IsNullOrEmpty(s.code) ? "" : s.code;
                    hd.tenkh = string.IsNullOrEmpty(s.name) ? "" : s.name;
                    hd.tenkhach = d.tenkhach;
                    hd.diachi = string.IsNullOrEmpty(s.address) ? "" : s.address;
                    hd.dienthoai = string.IsNullOrEmpty(s.telephone) ? "" : s.telephone;
                    hd.msthue = string.IsNullOrEmpty(s.taxcode) ? "" : s.taxcode;
                    hd.serial = d.serial;
                    hd.ghichu = d.ghichu;
                    hd.nguoitach = HttpContext.Session.GetString("username");
                    listhd.Add(hd);
                  

                    var da = new DataTuVetour();
                    da.stt = i;
                    da.diengiai = d.diengiai;
                    da.serial = d.serial;
                    da.xuatve = d.xuatve;
                    da.tenkhach = d.tenkhach;
                    da.sgtcode = d.sgtcode;
                    da.vat = 10;
                    da.tygia = tygia;
                    if (i < slhoadon -1)
                    {
                        da.sotiennt = tientach;
                        da.doanhthunn = doanhthutach;
                        tongtach += tientach;
                        tongdttach += doanhthutach;

                    }
                    else
                    {
                        da.sotiennt = tientourcoupon - tongtach;
                        da.doanhthunn = doanhthunn - doanhthutach;
                    }
                    da.loaitien = d.loaitien;
                    da.ghichu = dt.ToString("dd/MM/yyyy"); 
                    data.Add(da);
                    dt = dt.AddDays(1);
                }
            }
            
            
                        
            //s.date = System.DateTime.Now;
            ViewBag.countmkh = 1;
            //DateTime dt = System.DateTime.Now;
            //List<TachveViewModel> listhd = new List<TachveViewModel>();
            //for (int i = 0; i < slhoadon; i++)
            //{
            //    var hd = new TachveViewModel();
            //    hd.stt = i.ToString();
            //    hd.ngayct = dt.ToString("dd/MM/yyyy");
            //    hd.makh = string.IsNullOrEmpty(s.code) ? "" : s.code;
            //    hd.tenkh = string.IsNullOrEmpty(s.name) ? "" : s.name;
            //    hd.tenkhach = d.tenkhach;
            //    hd.diachi = string.IsNullOrEmpty(s.address) ? "" : s.address;
            //    hd.dienthoai = string.IsNullOrEmpty(s.telephone) ? "" : s.telephone;
            //    hd.msthue = string.IsNullOrEmpty(s.taxcode) ? "" : s.taxcode;
            //    hd.serial = d.serial;
            //    hd.ghichu = d.ghichu;
            //    hd.nguoitach = HttpContext.Session.GetString("username");
            //    listhd.Add(hd);
            //    dt = dt.AddDays(1);
            //}
            ViewBag.hoadon = listhd;
            // đếm số dòng của vé tour
            ViewBag.count = slhoadon;



            //List<DataTuVetour> data = new List<DataTuVetour>();
            //for (int i = 0; i < slhoadon; i++)
            //{
            //    var da = new DataTuVetour();
            //    da.stt = i;
            //    da.diengiai = d.diengiai;
            //    da.serial = d.serial;
            //    da.xuatve = d.xuatve;
            //    da.tenkhach = d.tenkhach;
            //    da.sgtcode = d.sgtcode;
            //    da.sotiennt = d.sotiennt;
            //    da.loaitien = d.loaitien;
            //    da.ghichu = d.ghichu;
            //    da.doanhthunn = d.doanhthunn;
            //    da.vat = d.vat;
            //    data.Add(da);
            //}
            ViewBag.tongtien = tientourcoupon;
            return View("Tachvetour", data);
        }
        [HttpPost]
        public ActionResult Tachvetour(string serial, decimal ppv, decimal tygia, string makh, string tkno, string tkco, string listhoadon, string listcthd)
        {
            ViewBag.serial = serial;
            string tour = serial.Substring(6, 2);
            switch (tour)
            {
                case "TO":
                    tour = "OB";
                    break;
                case "TN":
                    tour = "ND";
                    break;
                case "TW":
                    tour = "WI";
                    break;
                default:                   
                    break;
            }
            var hd = JsonConvert.DeserializeObject<List<TachveViewModel>>(listhoadon);
            var cthd = JsonConvert.DeserializeObject<List<DataTuVetour>>(listcthd);
            // var tthoadon = _dsdangkyhdRepository.getthongtinhd(HttpContext.Session.GetString("chinhanh"), _kyhieu);
            var s = _supplierRepository.GetByTwoKey(makh,HttpContext.Session.GetString("chinhanh"));
            if (s == null)
            {
                SetAlert("Khách hàng không tồn tại.", "error");
                return View("Tachvetour");
            }
            int rowhd = 0;
            foreach (var i in hd)
            {
                var h = new Tachve();
                // Tao stt tach ve
                h.Idhoadon = _tachveRepository.newId();
                string firstId = h.Idhoadon.Substring(0, 6);
                string lastId = h.Idhoadon.Substring(6, 4);
                h.stt = firstId + HttpContext.Session.GetString("maviettat") + lastId;
                h.ngayct = Convert.ToDateTime(i.ngayct);
                h.makh = i.makh;
                h.tenkh = i.tenkh;
                h.tenkhach = i.tenkhach;
                h.diachi = i.diachi;
                h.dienthoai = i.dienthoai;
                h.msthue = s.taxcode;
                h.ngaytao = System.DateTime.Now;
                h.nguoitach = HttpContext.Session.GetString("username");
                h.httt = "TM/CK";
                h.serial = i.serial;
                h.chinhanh = HttpContext.Session.GetString("chinhanh");
                h.tour = tour;// _tour;
                h.ghichu = i.ghichu;
                //h.nguoitaohd = _username;
                //h.user = _username;
                //h.keyhddt = h.hdvat.TrimStart(new char[] {'0'})+maviettat+ h.mausohd.Trim().Replace("/","") + h.kyhieu.Trim().Replace("/","");
                h.logfile = " User tách hoá đơn :" + HttpContext.Session.GetString("username") + " vào lúc " + System.DateTime.Now.ToString();
                _tachveRepository.Create(h);
                foreach (var c in cthd)
                {
                    if (rowhd == c.stt)
                    {
                        var ct = new Cttachve();
                        ct.Idhoadon = h.Idhoadon;
                        ct.diengiai = c.diengiai;
                        ct.serial = h.serial;
                        ct.xuatve = Convert.ToDateTime(c.xuatve);
                        ct.tenkhach = h.tenkhach;
                        ct.sgtcode = c.sgtcode;
                        ct.loaitien = c.loaitien;
                        ct.sotiennt = c.sotiennt;// (c.sotiennt) * tygia;
                        ct.tygia = tygia;
                        ct.sotien = c.sotiennt;// (c.sotiennt) * tygia;
                        ct.ppv = ppv;
                        ct.vat = c.vat;
                        ct.ghichu = h.ghichu;
                        ct.ngaytao = System.DateTime.Now;
                        ct.coupon = 0;
                        ct.tiencoupon = 0;
                        ct.tkno = tkno;
                        ct.tkco = tkco;
                        ct.tour = tour;
                        ct.httc = "";
                        ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                        ct.logfile = "===================" + System.Environment.NewLine + "User " + HttpContext.Session.GetString("username") + " thêm ct tách vé vào lúc: " + System.DateTime.Now.ToString();

                        _cttachveRepository.Create(ct);
                    }
                }
                rowhd++;
            }
            // return Redirect(HttpContext.Session.GetString("urlHoadon"));
           // return View("Hoadontach");
            return RedirectToAction("Hoadontach", new { tungay = DateTime.Now.ToString("dd/MM/yyyy"), denngay=DateTime.Now.ToString("dd/MM/yyyy") });
        }

        #endregion

        #region Hoa don tach
        public ActionResult Hoadontach(string tungay, string denngay)
        {
            tungay = tungay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            denngay = denngay ?? System.DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var t = _tachveRepository.listHoadontach(tungay, denngay, HttpContext.Session.GetString("chinhanh"));
            if (t == null)
            {
                return View("Hoadontach");
            }
            ViewBag.count = t.Count();
            return View("Hoadontach", t);
        }
        [HttpPost]
        public IActionResult Hoadontach(string listhoadontach)
        {
            var ht = JsonConvert.DeserializeObject<List<Tachve>>(listhoadontach);
            // var tthoadon = _dsdangkyhdRepository.getthongtinhd(HttpContext.Session.GetString("chinhanh"), _kyhieu);
            foreach (var tv in ht)
            {
                Tachve t = _tachveRepository.GetByTwoKey(tv.Idhoadon,HttpContext.Session.GetString("chinhanh"));
                Hoadon h = new Hoadon();
                h.Idhoadon = _hoadonRepository.newId(HttpContext.Session.GetString("chinhanh"));
                // h.stt = _hoadonRepository.newStt(tthoadon.maviettat);
                string firstId = h.Idhoadon.Substring(0, 6);
                string lastId = h.Idhoadon.Substring(6, 4);

                h.stt = firstId + HttpContext.Session.GetString("maviettat") + lastId;
                h.ngayct = t.ngayct;
                h.kyhieu = HttpContext.Session.GetString("kyhieuhd");
                h.makh = t.makh;
                h.tenkh = t.tenkh;
                h.tenkhach = t.tenkhach;
                h.diachi = t.diachi;
                h.dienthoai = t.dienthoai;
                h.msthue = t.msthue;
                h.hopdong = t.hopdong;
                h.ghichu = t.ghichu;
                h.user = t.nguoitach;
                h.ngaytao = System.DateTime.Now;
                h.serial = t.serial;
                h.nguoitaohd = HttpContext.Session.GetString("username");
                h.nguonhd = "Được chuyển từ hoá đơn tách Id:" + t.Idhoadon;
                h.mausohd = HttpContext.Session.GetString("mausohd");
                h.chinhanh = HttpContext.Session.GetString("chinhanh");
                h.logfile = " Hoá đơn được chuyển từ tách vé Id :" + t.Idhoadon + " user chuyển: " + HttpContext.Session.GetString("username") + " vào lúc " + System.DateTime.Now.ToString();
                // Thêm hoá đơn từ tách vé
                _hoadonRepository.Create(h);
                // Cập nhật lại chuyển vat và ngày chuyển của tách vé.
                t.chuyenvat = "Hoá đơn chuyển: " + h.Idhoadon;
                t.ngaychuyen = System.DateTime.Now;
                _tachveRepository.Update(t);
                List<Cttachve> listcttachve = _cttachveRepository.listCttachvebyId(tv.Idhoadon,HttpContext.Session.GetString("chinhanh")).ToList();
                foreach (var c in listcttachve)
                {
                    var ct = new cthdvat();
                    ct.Idhoadon = h.Idhoadon;
                    ct.chinhanh = HttpContext.Session.GetString("chinhanh");
                    ct.diengiai = c.diengiai;
                    ct.serial = c.serial;
                    ct.xuatve = c.xuatve;
                    ct.tenkhach = c.tenkhach;
                    ct.sgtcode = c.sgtcode;
                    ct.sotiennt = c.sotiennt;
                    ct.loaitien = c.loaitien;
                    ct.tygia = c.tygia;
                    ct.sotien = c.sotien;
                    ct.ppv = c.ppv;
                    ct.vat = c.vat;
                    ct.ghichu = c.ghichu;
                    ct.ngaytao = System.DateTime.Now;
                    ct.tkno = c.tkno;
                    ct.tkco = c.tkco;
                    ct.tour = c.tour;
                    ct.httc = c.httc;
                    ct.logfile = "- Thông tin được chuyển từ chi tiết tách vé Id: " + c.Id + ", Người chuyển: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now.ToString();
                    _cthdvatRepository.Create(ct);
                }

            }
            return RedirectToAction("Index");
        }

        public IActionResult listCttachve(string Idhoadon)
        {
            var ct = _cttachveRepository.listCttachvebyId(Idhoadon,HttpContext.Session.GetString("chinhanh"));
            return View("listCttachve", ct);
        }

        #endregion




        #region Xem logfile hoá đơn
        public ActionResult Viewloghoadon(string idhoadon)
        {
            var hd = _hoadonRepository.GetByTwoKey(idhoadon, HttpContext.Session.GetString("chinhanh"));
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

        public void listNguontour(string selected = "")
        {
            try
            {
                var tour = _nguonhdRepository.GetAll().Where(x=>x.active==true);                
                ViewBag.tour = new SelectList(tour, "IdNguonhd", "IdNguonhd", selected);
            }
            catch { return; }
        }
        public void dsMaviettat(string selected = "")
        {
            var a = _dsdangkyhdRepository.listDangkyhoadon();

            IEnumerable<SelectListItem> selectList = from s in a
                                                     select new SelectListItem
                                                     {
                                                         Value = s.chinhanh,
                                                         Text = s.chinhanh
                                                     };
            ViewBag.maviettat = new SelectList(selectList.Distinct(), "Value", "Text", selected);
        }
        #endregion




    }
}

