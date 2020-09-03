using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NumToWords;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
namespace hdvatob.Controllers
{
    public class InvoiceController : BaseController
    {
        private readonly IHoadonRepository _hoadonRepository;
        private readonly ICthdvatRepository _cthdvatRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IDsdangkyhdRepository _dsdangkyhdRepository;
        private readonly IHuyhdvatRepository _huyhdvatRepository;
        private readonly IHuycthdvatRepository _huycthdvatRepository;
        private readonly IUserRepository _userRepository;

        public InvoiceController(IHoadonRepository hoadonRepository, ICthdvatRepository cthdvatRepository,
                                 ISupplierRepository supplierRepository, IDsdangkyhdRepository dsdangkyhdRepository,
                                 IHuyhdvatRepository huyhdvatRepository, IHuycthdvatRepository huycthdvatRepository, IUserRepository userRepository)
        {
            _hoadonRepository = hoadonRepository;
            _cthdvatRepository = cthdvatRepository;
            _supplierRepository = supplierRepository;
            _dsdangkyhdRepository = dsdangkyhdRepository;
            _huyhdvatRepository = huyhdvatRepository;
            _huycthdvatRepository = huycthdvatRepository;
            _userRepository = userRepository;
        }

        public async Task<ActionResult> TaoHoadonVetour(string Idhoadon)
        {
            var user = _userRepository.GetById(HttpContext.Session.GetString("username"));
            string khhd = HttpContext.Session.GetString("kyhieuhd").Trim();
            string mausohd = HttpContext.Session.GetString("mausohd").Trim();
            var dkhd = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh == HttpContext.Session.GetString("maviettat")).SingleOrDefault();
            decimal mainkey = Convert.ToDecimal(dkhd.mainkey) + 1;
            string key = mainkey.ToString() + HttpContext.Session.GetString("maviettat") + mausohd.Replace("/", "") + khhd.Replace("/", "");// HttpContext.Session.GetString("mausohd").Replace("/","")+HttpContext.Session.GetString("kyhieuhd").Replace("/","");
            // Lấy thông tin site, user, pass add tự động
            string sitehddt = dkhd.sitehddt + "/PublishService.asmx";
            string usersite = dkhd.usersite;
            string passsite = dkhd.passsite;
            //string account = user.accounthddt;// HttpContext.Session.GetString("accounthddt");
            //string pass = user.passwordhddt;// HttpContext.Session.GetString("passhddt");

            var hd = _hoadonRepository.GetByTwoKey(Idhoadon, HttpContext.Session.GetString("chinhanh"));

            List<cthdvat> cthd = _cthdvatRepository.ListChitietHoadon(Idhoadon, HttpContext.Session.GetString("chinhanh")).Where(x => x.ngayhuy is null).ToList();
            string InvData_xml = "<Invoices><Inv><key>" + key + "</key><Invoice>";
            // Tạo thông tin người bán (không cần truyền tham số, VNPT sẽ tự lấy trên trang web    
            InvData_xml += "<SellerName></SellerName><SellerAddress></SellerAddress><SellerTaxCode></SellerTaxCode><SellerPhone></SellerPhone>";
            // Thông tin khách hàng
            InvData_xml += "<CusCode>" + (string.IsNullOrEmpty(hd.makh) ? "00000" : hd.makh.Trim()) + HttpContext.Session.GetString("maviettat") + "</CusCode><CusName>" + (string.IsNullOrEmpty(hd.tenkh) ? "" : hd.tenkh.Replace("&", "&amp;")) + "</CusName><Buyer>" + (string.IsNullOrEmpty(hd.tenkhach) ? "" : hd.tenkhach.Replace("&", "&amp;")) + "</Buyer><CusAddress>" + hd.diachi + "</CusAddress>";
            InvData_xml += "<CusTaxCode>" + hd.msthue + "</CusTaxCode><PaymentMethod>" + hd.httt + "</PaymentMethod><CusBankNo></CusBankNo><CusPhone>" + hd.dienthoai + "</CusPhone><ContNo></ContNo><VehicleNo></VehicleNo>";
            InvData_xml += "<ContractDate></ContractDate><DocDate>" + (hd.ngayct.HasValue ? hd.ngayct.Value.ToString("dd/MM/yyyy") : "") + "</DocDate><ContractNumber></ContractNumber><ReferenceNo></ReferenceNo>";

            //Tạo list Product
            InvData_xml += "<Products>";
            double tcvat = 0, tcsotien = 0, tcppv = 0;
            foreach (var i in cthd)
            {
                double tienthuevat = 0, doanhthu = 0, tienppv = 0; ;
                tienppv = Math.Round(0.01 * (double)i.sotien * (double)i.ppv / ((1 + 0.01 * (double)i.ppv) * (1 + 0.01 * i.vat)), 0);
                tienthuevat = Math.Round((double)i.sotien * 0.01 * i.vat / (1 + 0.01 * i.vat), 0);
                doanhthu = Math.Round((double)i.sotien - tienthuevat - tienppv, 0);

                InvData_xml += "<Product>";
                InvData_xml += "<ProdName>" + i.diengiai + "</ProdName>";
                InvData_xml += "<ProdUnit>" + (i.sk == 0 ? 1 : i.sk) + "</ProdUnit>";
                InvData_xml += "<ProdQuantity>" + (i.slve == 0 ? 1 : i.slve) + "</ProdQuantity>";
                InvData_xml += "<ProdPrice>" + i.sotien + "</ProdPrice>";
                InvData_xml += "<Total>" + doanhthu + "</Total>";// Thành tiền chưa thuế vat
                InvData_xml += "<Extra1>" + tienppv + "</Extra1>"; // Tiền phí phục vụ
                InvData_xml += "<VATRate>" + i.vat + "</VATRate>"; // Thuế vat                
                tcvat += tienthuevat;
                tcsotien += doanhthu;
                tcppv += tienppv;
                //tcsotienvathue += tienthuevat;
                InvData_xml += "<VATAmount>" + tienthuevat + "</VATAmount>"; // Tiền thuế
                InvData_xml += "<Amount>" + i.sotien + "</Amount>"; // Thành tiền có thuế
                InvData_xml += "</Product>";
            }
            //Đóng tag Product
            InvData_xml += "</Products>";
            //Thông tin báo cáo
            InvData_xml += "<GrossValue>" + tcsotien + "</GrossValue>"; // *Tổng tiền trước thuế của mức thuế null (không chịu thuế) NUMBER(18,4)
            InvData_xml += "<GrossValue0>0</GrossValue0>";//*Tổng tiền trước thuế của mức thuế 0% NUMBER(18,4)
            InvData_xml += "<GrossValue5>0</GrossValue5>"; //*Tổng tiền trước thuế của mức thuế 5 % NUMBER(18, 4)
            InvData_xml += "<GrossValue10>0</GrossValue10>";//*Tổng tiền trước thuế của mức thuế 10 % NUMBER(18, 4)
            InvData_xml += "<VatAmount0>0</VatAmount0>";//*Tổng tiền thuế của mức thuế 0% NUMBER(18,4)
            InvData_xml += "<VatAmount5>0</VatAmount5>";//*Tổng tiền thuế của mức thuế 5% NUMBER(18,4)
            InvData_xml += "<VatAmount10>" + tcvat + "</VatAmount10>";//Tổng tiền thuế của mức thuế 10% NUMBER(18,4)
            InvData_xml += "<TotalQuantity>" + tcppv + "</TotalQuantity>";//Tổng tiền phí phục vụ NUMBER(38,4)
            InvData_xml += "<Total>" + tcsotien + "</Total>";//Cộng tiền hàng (Sub -total)* NUMBER(38,4)
            InvData_xml += "<VATRate>" + 10 + "</VATRate>";// Thuế GTGT* FLOAT
            InvData_xml += "<VATAmount>" + tcvat + "</VATAmount>";//Tổng Tiền thuế GTGT* NUMBER(38,4)
            InvData_xml += "<Amount>" + (tcsotien + tcvat + tcppv) + "</Amount>";// Tổng cộng tiền thanh toán(Grand total)* NUMBER(38,4)
            string a = SoSangChu.DoiSoSangChu((tcsotien + tcvat + tcppv).ToString());
            a = char.ToUpper(a[0]) + a.Substring(1);
            // InvData_xml += "<AmountInWords>" + SoSangChu.DoiSoSangChu((tcsotien + tcvat + tcppv).ToString()) + " đồng </AmountInWords>";//	<!--Số tiền viết bằng chữ* VARCHAR2(255 CHAR)-->";
            InvData_xml += "<AmountInWords>" + a + " đồng </AmountInWords>";//	<!--Số tiền viết bằng chữ* VARCHAR2(255 CHAR)-->";
            InvData_xml += "<ArisingDate>" + System.DateTime.Now.ToString("dd/MM/yyyy") + "</ArisingDate>";// Ngày hóa đơn (dd/MM/yyyy) VARCHAR2(50 CHAR)

            //Đóng tag Invoice
            InvData_xml += "</Invoice></Inv></Invoices>";
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(InvData_xml);
            //string Json= JsonConvert.SerializeXmlNode(doc);

            //string xml = JsonConvert.DeserializeXmlNode(Json).OuterXml;

            var inv = new PublishService.PublishServiceSoapClient(PublishService.PublishServiceSoapClient.EndpointConfiguration.PublishServiceSoap);

            // Hàm add webservice động
            inv.ChannelFactory.Endpoint.Address = new EndpointAddress(sitehddt);

            //Task<PublishService.ImportAndPublishInvResponse> ketqua = inv.ImportAndPublishInvAsync(HttpContext.Session.GetString("accounthddt"), HttpContext.Session.GetString("passhddt"), InvData_xml, usersite, passsite, mausohd, khhd, 0);// HttpContext.Session.GetString("masohd"), HttpContext.Session.GetString("kyhieuhd"), 0);
            Task<PublishService.ImportAndPublishInvResponse> ketqua = inv.ImportAndPublishInvAsync(user.accounthddt, user.passwordhddt, InvData_xml, usersite, passsite, mausohd, khhd, 0);// HttpContext.Session.GetString("masohd"), HttpContext.Session.GetString("kyhieuhd"), 0);

            var result = await ketqua;

            if (result.Body.ImportAndPublishInvResult.Substring(0, 2) == "OK")
            {

                // Cập nhật lại hoá đơn
                // 
                string sohoadon = result.Body.ImportAndPublishInvResult.Split("_").Last();
                hd.hdvat = sohoadon.PadLeft(7, '0');
                //hd.hdvat =  mainkey.ToString().PadLeft(7, '0');
                hd.keyhddt = key;
                hd.ngayct = System.DateTime.Now;
                hd.ngayin = System.DateTime.Now;
                hd.datelock = System.DateTime.Now;
                hd.locker = HttpContext.Session.GetString("username");
                hd.user = HttpContext.Session.GetString("username");
                hd.logfile += hd.logfile + System.Environment.NewLine + " * User xuất hoá đơn: " + hd.user + " vào lúc: " + System.DateTime.Now;
                _hoadonRepository.Update(hd);
                SetAlert("Xuất hoá đơn thành công.", "success");
                // Cập nhật mainkey cho table dsdangkyhoadon
                // int id = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh==HttpContext.Session.GetString("maviettat")).SingleOrDefault().id;
                _dsdangkyhdRepository.updateMainkey(dkhd.id);
                _dsdangkyhdRepository.updateSohoadon(dkhd.id, decimal.Parse(sohoadon));
            }
            else
            {
                //switch (result.Body.ImportAndPublishInvResult.Substring(0, 5))
                switch (result.Body.ImportAndPublishInvResult.ToString())
                {
                    case "ERR:1":
                        SetAlert("User đăng nhập sai, hoặc không có quyền tạo hoá đơn", "error");
                        break;
                    case "ERR:3":
                        SetAlert("Dữ liệu đưa vào hoá đơn sai", "error");
                        break;
                    case "ERR:5":
                        SetAlert("Không phát hành được hoá đơn", "error");
                        break;
                    case "ERR:6":
                        SetAlert("Số hoá đơn vượt mức đăng ký phát hành", "error");
                        break;
                    case "ERR:7":
                        SetAlert("User name sai", "error");
                        break;
                    case "ERR:10":
                        SetAlert("Số hoá đơn vượt quá mức đăng ký phát hành", "error");
                        break;
                    case "ERR:13":
                        // int id = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh == HttpContext.Session.GetString("maviettat")).SingleOrDefault().id;
                        SetAlert("Trùng mã tra cứu, vui lòng xuất lại hoá đơn", "error");
                        _dsdangkyhdRepository.updateMainkey(dkhd.id);

                        break;
                    default:
                        SetAlert("Không tạo được hoá đơn", "error");
                        break;
                }
            }

            //return Json(result);
            return Redirect(HttpContext.Session.GetString("urlEditHoadon"));

        }
        public async Task<ActionResult> Huyhoadon(string Idhoadon)
        {
            var user = _userRepository.GetById(HttpContext.Session.GetString("username"));
            var hd = _hoadonRepository.GetByTwoKey(Idhoadon, HttpContext.Session.GetString("chinhanh"));
            var dkhd = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh == HttpContext.Session.GetString("maviettat")).SingleOrDefault();
            string sitehddt = dkhd.sitehddt + "/BusinessService.asmx";
            string usersite = dkhd.usersite;
            string passsite = dkhd.passsite;
            //string account = HttpContext.Session.GetString("accounthddt");
            //string pass = HttpContext.Session.GetString("passhddt"); 

            var huyhd = new BusinessService.BusinessServiceSoapClient(BusinessService.BusinessServiceSoapClient.EndpointConfiguration.BusinessServiceSoap12);

            // Hàm add webservice động
            huyhd.ChannelFactory.Endpoint.Address = new EndpointAddress(sitehddt);

            //Task<BusinessService.cancelInvResponse> ketqua = huyhd.cancelInvAsync(HttpContext.Session.GetString("accounthddt"), HttpContext.Session.GetString("passhddt"), hd.keyhddt, usersite, passsite);
            Task<BusinessService.cancelInvResponse> ketqua = huyhd.cancelInvAsync(user.accounthddt, user.passwordhddt, hd.keyhddt, usersite, passsite);
            var result = await ketqua;
            if (result.Body.cancelInvResult.Substring(0, 2) == "OK")
            {
                _hoadonRepository.huyhoadontrongthang(hd.Idhoadon, hd.chinhanh);
                hd.logfile += hd.logfile + System.Environment.NewLine + " * User huỷ hoá đơn: " + HttpContext.Session.GetString("username") + " key hoá đơn điện tử: " + hd.keyhddt + " vào lúc: " + System.DateTime.Now;
                hd.keyhddt = "";
                hd.hdvat = "";
                hd.ngayin = null;
                hd.ngayxoa = System.DateTime.Now;
                hd.locker = "";
                hd.datelock = null;
                _hoadonRepository.Update(hd);
                SetAlert("Huỷ hoá đơn thành công.", "success");
            }
            else
            {
                switch (result.Body.cancelInvResult.Substring(0, 5))
                {
                    case "ERR:1":
                        SetAlert("User name không đúng, hoặc không có quyền", "error");
                        break;
                    case "ERR:2":
                        SetAlert("Không tồn tại hoá đơn cần huỷ", "error");
                        break;
                    case "ERR:8":
                        SetAlert("Hoá đơn này đã được huỷ rồi", "error");
                        break;
                    case "ERR:9":
                        SetAlert("Trạng thái hoá đơn không được huỷ", "error");
                        break;
                    default:
                        SetAlert("Không huỷ được hoá đơn này", "error");
                        break;
                }
            }

            return Redirect(HttpContext.Session.GetString("urlEditHoadon"));
        }

        public async Task<ActionResult> TaoHoadonHuy(string Idhoadon)
        {
            var user = _userRepository.GetById(HttpContext.Session.GetString("username"));
            string khhd = HttpContext.Session.GetString("kyhieuhd").Trim();
            string mausohd = HttpContext.Session.GetString("mausohd").Trim();
            var dkhd = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh == HttpContext.Session.GetString("maviettat")).SingleOrDefault();
            decimal mainkey = Convert.ToDecimal(dkhd.mainkey) + 1;
            string key = mainkey.ToString() + HttpContext.Session.GetString("maviettat") + mausohd.Replace("/", "") + khhd.Replace("/", "");// HttpContext.Session.GetString("mausohd").Replace("/","")+HttpContext.Session.GetString("kyhieuhd").Replace("/","");
            // Lấy thông tin site, user, pass add tự động
            string sitehddt = dkhd.sitehddt + "/PublishService.asmx";
            string usersite = dkhd.usersite;
            string passsite = dkhd.passsite;


            var hd = _huyhdvatRepository.GetByTwoKey(Idhoadon, HttpContext.Session.GetString("chinhanh"));

            List<Huycthdvat> cthd = _huycthdvatRepository.Listhuycthdvat(Idhoadon, HttpContext.Session.GetString("chinhanh")).Where(x => x.ngayhuy is null).ToList();
            string InvData_xml = "<Invoices><Inv><key>" + key + "</key><Invoice>";
            // Tạo thông tin người bán (không cần truyền tham số, VNPT sẽ tự lấy trên trang web    
            InvData_xml += "<SellerName></SellerName><SellerAddress></SellerAddress><SellerTaxCode></SellerTaxCode><SellerPhone></SellerPhone>";
            // Thông tin khách hàng
            InvData_xml += "<CusCode>" + (string.IsNullOrEmpty(hd.makh) ? "00000" : hd.makh.Trim()) + HttpContext.Session.GetString("maviettat") + "</CusCode><CusName>" + (string.IsNullOrEmpty(hd.tenkh) ? "" : hd.tenkh.Replace("&", "&amp;")) + "</CusName><Buyer>" + (string.IsNullOrEmpty(hd.tenkhach) ? "" : hd.tenkhach.Replace("&", "&amp;")) + " </Buyer><CusAddress>" + hd.diachi + "</CusAddress>";
            InvData_xml += "<CusTaxCode>" + hd.msthue + "</CusTaxCode><PaymentMethod>" + hd.httt + "</PaymentMethod><CusBankNo></CusBankNo><CusPhone>" + hd.dienthoai + "</CusPhone><ContNo></ContNo><VehicleNo></VehicleNo>";
            InvData_xml += "<ContractDate></ContractDate><DocDate>" + (hd.ngayct.HasValue ? hd.ngayct.Value.ToString("dd/MM/yyyy") : "") + "</DocDate><ContractNumber></ContractNumber><ReferenceNo></ReferenceNo>";

            //Tạo list Product
            InvData_xml += "<Products>";
            double tcvat = 0, tcsotien = 0, tcppv = 0;
            foreach (var i in cthd)
            {
                double tienthuevat = 0, doanhthu = 0, tienppv = 0; ;
                tienppv = Math.Round(0.01 * (double)i.sotien * (double)i.ppv / ((1 + 0.01 * (double)i.ppv) * (1 + 0.01 * i.vat)), 0);
                tienthuevat = Math.Round((double)i.sotien * 0.01 * i.vat / (1 + 0.01 * i.vat), 0);
                doanhthu = Math.Round((double)i.sotien - tienthuevat - tienppv, 0);

                InvData_xml += "<Product>";
                InvData_xml += "<ProdName>" + i.diengiai + "</ProdName>";
                InvData_xml += "<ProdUnit>" + (i.sk == 0 ? 1 : i.sk) + "</ProdUnit>";
                InvData_xml += "<ProdQuantity>" + (i.slve == 0 ? 1 : i.slve) + "</ProdQuantity>";
                InvData_xml += "<ProdPrice>" + Math.Abs(i.sotien) + "</ProdPrice>";
                InvData_xml += "<Total>" + Math.Abs(doanhthu) + "</Total>";// Thành tiền chưa thuế vat
                InvData_xml += "<Extra1>" + Math.Abs(tienppv) + "</Extra1>"; // Tiền phí phục vụ
                InvData_xml += "<VATRate>" + i.vat + "</VATRate>"; // Thuế vat                
                tcvat += tienthuevat;
                tcsotien += doanhthu;
                tcppv += tienppv;
                //tcsotienvathue += tienthuevat;
                InvData_xml += "<VATAmount>" + Math.Abs(tienthuevat) + "</VATAmount>"; // Tiền thuế
                InvData_xml += "<Amount>" + Math.Abs(i.sotien) + "</Amount>"; // Thành tiền có thuế
                InvData_xml += "</Product>";
            }

            //Đóng tag Product
            InvData_xml += "</Products>";
            //Thông tin báo cáo
            InvData_xml += "<GrossValue>" + Math.Abs(tcsotien) + "</GrossValue>"; // *Tổng tiền trước thuế của mức thuế null (không chịu thuế) NUMBER(18,4)
            InvData_xml += "<GrossValue0>0</GrossValue0>";//*Tổng tiền trước thuế của mức thuế 0% NUMBER(18,4)
            InvData_xml += "<GrossValue5>0</GrossValue5>"; //*Tổng tiền trước thuế của mức thuế 5 % NUMBER(18, 4)
            InvData_xml += "<GrossValue10>0</GrossValue10>";//*Tổng tiền trước thuế của mức thuế 10 % NUMBER(18, 4)
            InvData_xml += "<VatAmount0>0</VatAmount0>";//*Tổng tiền thuế của mức thuế 0% NUMBER(18,4)
            InvData_xml += "<VatAmount5>0</VatAmount5>";//*Tổng tiền thuế của mức thuế 5% NUMBER(18,4)
            InvData_xml += "<VatAmount10>" + Math.Abs(tcvat) + "</VatAmount10>";//Tổng tiền thuế của mức thuế 10% NUMBER(18,4)
            InvData_xml += "<TotalQuantity>" + Math.Abs(tcppv) + "</TotalQuantity>";//Tổng tiền phí phục vụ NUMBER(38,4)
            InvData_xml += "<Total>" + Math.Abs(tcsotien) + "</Total>";//Cộng tiền hàng (Sub -total)* NUMBER(38,4)
            InvData_xml += "<VATRate>" + 10 + "</VATRate>";// Thuế GTGT* FLOAT
            InvData_xml += "<VATAmount>" + Math.Abs(tcvat) + "</VATAmount>";//Tổng Tiền thuế GTGT* NUMBER(38,4)
            InvData_xml += "<Amount>" + Math.Abs((tcsotien + tcvat + tcppv)) + "</Amount>";// Tổng cộng tiền thanh toán(Grand total)* NUMBER(38,4)
            double t = Math.Abs(tcsotien + tcvat + tcppv);
            string a = SoSangChu.DoiSoSangChu(t.ToString());
            a = char.ToUpper(a[0]) + a.Substring(1);
            InvData_xml += "<AmountInWords>" + a + " đồng </AmountInWords>";//	<!--Số tiền viết bằng chữ* VARCHAR2(255 CHAR)-->";
            //InvData_xml += "<AmountInWords>" + SoSangChu.DoiSoSangChu(Math.Abs((tcsotien + tcvat + tcppv)).ToString()) + " đồng </AmountInWords>";//	<!--Số tiền viết bằng chữ* VARCHAR2(255 CHAR)-->";
            InvData_xml += "<ArisingDate>" + System.DateTime.Now.ToString("dd/MM/yyyy") + "</ArisingDate>";// Ngày hóa đơn (dd/MM/yyyy) VARCHAR2(50 CHAR)

            //Đóng tag Invoice
            InvData_xml += "</Invoice></Inv></Invoices>";
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(InvData_xml);
            //string Json= JsonConvert.SerializeXmlNode(doc);

            //string xml = JsonConvert.DeserializeXmlNode(Json).OuterXml;

            var inv = new PublishService.PublishServiceSoapClient(PublishService.PublishServiceSoapClient.EndpointConfiguration.PublishServiceSoap);

            // Hàm add webservice động
            inv.ChannelFactory.Endpoint.Address = new EndpointAddress(sitehddt);


            //Task<PublishService.ImportAndPublishInvResponse> ketqua = inv.ImportAndPublishInvAsync(HttpContext.Session.GetString("accounthddt"), HttpContext.Session.GetString("passhddt"), InvData_xml, usersite, passsite, mausohd, khhd, 0);// HttpContext.Session.GetString("masohd"), HttpContext.Session.GetString("kyhieuhd"), 0);
            Task<PublishService.ImportAndPublishInvResponse> ketqua = inv.ImportAndPublishInvAsync(user.accounthddt, user.passwordhddt, InvData_xml, usersite, passsite, mausohd, khhd, 0);// HttpContext.Session.GetString("masohd"), HttpContext.Session.GetString("kyhieuhd"), 0);
            var result = await ketqua;

            if (result.Body.ImportAndPublishInvResult.Substring(0, 2) == "OK")
            {

                // Cập nhật lại hoá đơn
                // 
                string sohoadon = result.Body.ImportAndPublishInvResult.Split("_").Last();
                hd.hdvat = sohoadon.PadLeft(7, '0');
                //hd.hdvat =  mainkey.ToString().PadLeft(7, '0');
                hd.keyhddt = key;
                hd.ngayin = System.DateTime.Now;
                hd.datelock = System.DateTime.Now;
                hd.locker = HttpContext.Session.GetString("username");
                // hd.user = HttpContext.Session.GetString("username");
                hd.logfile += hd.logfile + System.Environment.NewLine + " * User xuất hoá đơn: " + HttpContext.Session.GetString("username") + " vào lúc: " + System.DateTime.Now;
                _huyhdvatRepository.Update(hd);
                SetAlert("Xuất hoá đơn thành công.", "success");
                // Cập nhật mainkey cho table dsdangkyhoadon
                // int id = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh == HttpContext.Session.GetString("maviettat")).SingleOrDefault().id;
                _dsdangkyhdRepository.updateMainkey(dkhd.id);
                _dsdangkyhdRepository.updateSohoadon(dkhd.id, decimal.Parse(sohoadon));
            }
            else
            {
                //switch (result.Body.ImportAndPublishInvResult.Substring(0, 5))
                switch (result.Body.ImportAndPublishInvResult.ToString())
                {
                    case "ERR:1":
                        SetAlert("User đăng nhập sai, hoặc không có quyền tạo hoá đơn", "error");
                        break;
                    case "ERR:3":
                        SetAlert("Dữ liệu đưa vào hoá đơn sai", "error");
                        break;
                    case "ERR:5":
                        SetAlert("Không phát hành được hoá đơn", "error");
                        break;
                    case "ERR:6":
                        SetAlert("Số hoá đơn vượt mức đăng ký phát hành", "error");
                        break;
                    case "ERR:7":
                        SetAlert("User name sai", "error");
                        break;
                    case "ERR:10":
                        SetAlert("Số hoá đơn vượt quá mức đăng ký phát hành", "error");
                        break;
                    case "ERR:13":
                        // int id = _dsdangkyhdRepository.listDangkyhoadon().Where(x => x.kyhieuhd == HttpContext.Session.GetString("kyhieuhd") && x.chinhanh == HttpContext.Session.GetString("maviettat")).SingleOrDefault().id;                      
                        SetAlert("Trùng mã tra cứu, vui lòng xuất lại hoá đơn", "error");
                        _dsdangkyhdRepository.updateMainkey(dkhd.id);
                        break;
                    default:
                        SetAlert("Không tạo được hoá đơn", "error");
                        break;

                }
            }
            return Redirect(HttpContext.Session.GetString("urlEditHoadonhuy"));
        }
        public ActionResult PreviewHoadon(string Idhoadon)
        {
            var hd = _hoadonRepository.GetByTwoKey(Idhoadon, HttpContext.Session.GetString("chinhanh"));

            ViewBag.sohd = hd.hdvat;
            List<cthdvat> cthd = _cthdvatRepository.ListChitietHoadon(Idhoadon, HttpContext.Session.GetString("chinhanh")).Where(x => x.ngayhuy is null).ToList();
            List<ProductsViewModel> products = new List<ProductsViewModel>();
            double tcvat = 0, tcsotien = 0, tcppv = 0;
            int stt = 0;
            foreach (var i in cthd)
            {
                double tienthuevat = 0, doanhthu = 0, tienppv = 0;
                tienppv = Math.Round(0.01 * (double)i.sotien * (double)i.ppv / ((1 + 0.01 * (double)i.ppv) * (1 + 0.01 * i.vat)), 0);
                tienthuevat = Math.Round((double)i.sotien * 0.01 * i.vat / (1 + 0.01 * i.vat), 0);
                doanhthu = Math.Round((double)i.sotien - tienthuevat - tienppv, 0);
                var p = new ProductsViewModel();
                tcvat += tienthuevat;
                tcsotien += doanhthu;
                tcppv += tienppv;
                stt += 1;
                products.Add(new ProductsViewModel()
                {
                    ProdId = stt,
                    ProdName = i.diengiai,
                    ProdUnit = (i.sk == 0 ? 1 : i.sk),
                    ProdQuantity = (i.slve == 0 ? 1 : i.slve),
                    Total = doanhthu,
                    Extra1 = tienppv,
                    VATRate = i.vat,
                    VATAmount = tienthuevat,
                    Amount = doanhthu
                }); ;
            }
            ViewBag.count = products.Count();
            ViewBag.listProduct = products;
            ViewBag.tong = (tcsotien + tcvat + tcppv);
            ViewBag.tcsotien = tcsotien;
            ViewBag.tcvat = tcvat;
            ViewBag.tcppv = tcppv;
            string a = SoSangChu.DoiSoSangChu((tcsotien + tcvat + tcppv).ToString());
            a = char.ToUpper(a[0]) + a.Substring(1);
            // ViewBag.sotienbangchu = SoSangChu.DoiSoSangChu((tcsotien + tcvat + tcppv).ToString())+ " đồng.";
            ViewBag.sotienbangchu = a + " đồng.";
            return PartialView("PreviewHoadon", hd);
        }

        public ActionResult PreviewHoadonHuy(string Idhoadon, string chinhanh)
        {
            var hd = _huyhdvatRepository.GetByTwoKey(Idhoadon, chinhanh);// HttpContext.Session.GetString("chinhanh"));

            ViewBag.sohd = hd.hdvat;
            List<Huycthdvat> cthd = _huycthdvatRepository.Listhuycthdvat(Idhoadon, HttpContext.Session.GetString("chinhanh")).Where(x => x.ngayhuy is null).ToList();
            List<ProductsViewModel> products = new List<ProductsViewModel>();
            double tcvat = 0, tcsotien = 0, tcppv = 0;
            int stt = 0;
            foreach (var i in cthd)
            {
                double tienthuevat = 0, doanhthu = 0, tienppv = 0;
                tienppv = Math.Round(0.01 * (double)i.sotien * (double)i.ppv / ((1 + 0.01 * (double)i.ppv) * (1 + 0.01 * i.vat)), 0);
                tienthuevat = Math.Round((double)i.sotien * 0.01 * i.vat / (1 + 0.01 * i.vat), 0);
                doanhthu = Math.Round((double)i.sotien - tienthuevat - tienppv, 0);
                var p = new ProductsViewModel();
                tcvat += tienthuevat;
                tcsotien += doanhthu;
                tcppv += tienppv;
                stt += 1;
                products.Add(new ProductsViewModel()
                {
                    ProdId = stt,
                    ProdName = i.diengiai,
                    ProdUnit = (i.sk == 0 ? 1 : i.sk),
                    ProdQuantity = (i.slve == 0 ? 1 : i.slve),
                    Total = Math.Abs(doanhthu),
                    Extra1 = Math.Abs(tienppv),
                    VATRate = i.vat,
                    VATAmount = Math.Abs(tienthuevat),
                    Amount = Math.Abs(doanhthu)
                }); ; ;
            }
            ViewBag.count = products.Count();
            ViewBag.listProduct = products;
            ViewBag.tong = Math.Abs((tcsotien + tcvat + tcppv));
            ViewBag.tcsotien = Math.Abs(tcsotien);
            ViewBag.tcvat = Math.Abs(tcvat);
            ViewBag.tcppv = Math.Abs(tcppv);
            double t = Math.Abs(tcsotien + tcvat + tcppv);
            //string a = SoSangChu.DoiSoSangChu((tcsotien + tcvat + tcppv).ToString());
            string a = SoSangChu.DoiSoSangChu((t).ToString());
            a = char.ToUpper(a[0]) + a.Substring(1);
            // ViewBag.sotienbangchu = SoSangChu.DoiSoSangChu(tcsotien + tcvat + tcppv).ToString()) + " đồng.";
            ViewBag.sotienbangchu = a + " đồng.";
            return PartialView("PreviewHoadonHuy", hd);
        }

    }

}