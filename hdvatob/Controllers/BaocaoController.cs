using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using hdvatob.Data.Interfaces;
using hdvatob.ViewModel;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Novacode;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using NumToWords;
using Microsoft.AspNetCore.Http;
using hdvatob.Data.Model;

namespace hdvatob.Controllers
{
    public class BaocaoController : Controller
    {
        private readonly IBaocaoRepository _baocaoRepository;
        private readonly IHoadonRepository _hoadonRepository;
        private readonly ICthdvatRepository _cthdvatRepository;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly IDsdangkyhdRepository _dsdangkyhdRepository;
        private readonly INguonhdRepository _nguonhdRepository;

        //string _chinhanh,_kyhieu = "";
        string fromTo = "";
        public BaocaoController(IBaocaoRepository baocaoRepository, IHoadonRepository hoadonRepository, ICthdvatRepository cthdvatRepository, 
                                HostingEnvironment hostingEnvironment, IDsdangkyhdRepository dsdangkyhdRepository,INguonhdRepository nguonhdRepository)
        {
            _baocaoRepository = baocaoRepository;
            _hoadonRepository = hoadonRepository;
            _cthdvatRepository = cthdvatRepository;
            _hostingEnvironment = hostingEnvironment;         
            _dsdangkyhdRepository = dsdangkyhdRepository;
            _nguonhdRepository = nguonhdRepository;
            //_chinhanh = "STS";// HttpContext.Session.GetString("chinhanh");
            //_kyhieu = "NN";// HttpContext.Session.GetString("kyhieu");
        }
        #region Doanh thu ngày hoá đơn
        [HttpGet]
        public ActionResult doanhthungayhd(string tungay,string denngay,string tour)
        {
            tour = tour ?? "";
            ViewBag.tour = tour;
          //  listDatatour(tour);
            listNguontour(tour);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("doanhthungayhd");
            }
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            
            var d = _baocaoRepository.listDoanhthungayhd(tungay, denngay,tour, HttpContext.Session.GetString("chinhanh"));
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("doanhthungayhd");
            }
            else
            {              
                return View("doanhthungayhd", d);
            }
           
        }
        [HttpPost]
        public ActionResult doanhthungayhdToExcel(string tungay, string denngay,string tour)
        {
            tour = tour ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listDoanhthungayhd(tungay, denngay, tour, HttpContext.Session.GetString("chinhanh"));

          
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU NGÀY LẬP HOÁ ĐƠN ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2,9].Merge = true;
           
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "ngày " + tungay;
            }
            else
            {
                fromTo = "từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = "Các hoá đơn " + fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 9].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Ngày HĐ";
            xlSheet.Cells[5, 3].Value = "Số HĐ VAT";
            xlSheet.Cells[5, 4].Value = "Khách hàng";
            xlSheet.Cells[5, 5].Value = "Diễn giải";
            xlSheet.Cells[5, 6].Value = "Code đoàn";
            xlSheet.Cells[5, 7].Value = "Số vé tour";
            xlSheet.Cells[5, 8].Value = "Doanh thu";
            xlSheet.Cells[5, 9].Value = "%PPV";

            xlSheet.Cells[5, 10].Value = "Tiền PPV";
            xlSheet.Cells[5, 11].Value = "@VAT";
            xlSheet.Cells[5, 12].Value = "Thuế VAT";
            xlSheet.Cells[5, 13].Value = "Tổng cộng";
            xlSheet.Cells[5, 14].Value = "SK";

            xlSheet.Cells[5, 1, 5, 14].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 14].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 14].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (DoanhthungayhdViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.ngayhoadon.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.hdvat;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.tenkhach;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.diengiai;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 7].Value = vm.serial;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.doanhthu;
                xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 9].Value = vm.ppv;
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 10].Value = vm.tienppv;
                xlSheet.Cells[iRowIndex, 10].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 11].Value = vm.vat;
                TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 12].Value = vm.thuevat;
                xlSheet.Cells[iRowIndex, 12].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 13].Value = vm.tongcong;
                xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 14].Value = vm.sk;
                TrSetCellBorder(xlSheet, iRowIndex, 14, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 14].Style.Border.Right.Style = ExcelBorderStyle.Thin;


                iRowIndex += 1;
                idem += 1;
            }

            //dong tong
            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Value = "Tổng cộng";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Formula = "=SUM(H6:H" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 9].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 9].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 11].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 11].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 12].Formula = "=SUM(L6:L" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 12].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 12].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 13].Formula = "=SUM(M6:M" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 13].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 14].Formula = "=SUM(N6:N" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 14].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 14, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 14].Style.Border.Top.Style = ExcelBorderStyle.None;



            //end dong tong

            xlSheet.Cells.AutoFitColumns();
            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_dt_ngayhd" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion
        #region Doanh thu ngày huỷ hoá đơn
        [HttpGet]
        public ActionResult ngayhuyhd(string tungay, string denngay,string tour)
        {
            tour = tour ?? "";
            ViewBag.tour = tour;
            listNguontour(tour);
            if (string.IsNullOrEmpty(tungay) || string.IsNullOrEmpty(denngay))
            {
                return View("ngayhuyhd");
            }
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listNgayhuyhd(tungay, denngay,tour, "STS");
            ViewBag.count = d.Count();
            if (d == null)
            {
                return View("ngayhuyhd");
            }
            else
            {
                return View("ngayhuyhd", d);
            }

        }
        [HttpPost]
        public ActionResult ngayhuyhdToExcel(string tungay, string denngay,string tour)
        {
            tour = tour ?? "";
            ViewBag.tungay = tungay;
            ViewBag.denngay = denngay;
            var d = _baocaoRepository.listNgayhuyhd(tungay, denngay, tour, HttpContext.Session.GetString("chinhanh"));

            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");

            xlSheet.Cells[1, 1].Value = "CÔNG TY DỊCH VỤ LỮ HÀNH SAIGONTOURIST ";
            xlSheet.Cells[1, 1].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            xlSheet.Cells[1, 1, 1, 5].Merge = true;

            xlSheet.Cells[2, 1].Value = "BÁO CÁO GIẢM DOANH THU DO HUỶ HOÁ ĐƠN ";
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 18, FontStyle.Regular));
            xlSheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[2, 1, 2, 9].Merge = true;

            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "ngày " + tungay;
            }
            else
            {
                fromTo = "từ ngày " + tungay + " đến ngày " + denngay;
            }
            xlSheet.Cells[3, 1].Value = "Các hoá đơn huỷ " + fromTo;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 12, FontStyle.Regular));
            xlSheet.Cells[3, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            xlSheet.Cells[3, 1, 3, 9].Merge = true;

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Ngày HĐ";
            xlSheet.Cells[5, 3].Value = "Số HĐ VAT";
            xlSheet.Cells[5, 4].Value = "Khách hàng";
            xlSheet.Cells[5, 5].Value = "Diễn giải";
            xlSheet.Cells[5, 6].Value = "Code đoàn";
            xlSheet.Cells[5, 7].Value = "Số vé tour";
            xlSheet.Cells[5, 8].Value = "Số tiền NT";
            xlSheet.Cells[5, 9].Value = "Tiền VNĐ";
            xlSheet.Cells[5, 10].Value = "%PPV";
            xlSheet.Cells[5, 11].Value = "Tiền PPV";
            xlSheet.Cells[5, 12].Value = "@VAT";
            xlSheet.Cells[5, 13].Value = "Thuế VAT";
            

            xlSheet.Cells[5, 1, 5, 13].Style.Font.SetFromFont(new System.Drawing.Font("Times New Roman", 10, FontStyle.Regular));
            xlSheet.Cells[5, 1, 5, 13].Style.WrapText = false;
            xlSheet.Cells[5, 1, 5, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // Set Border
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            // Set màu ch Border
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Left.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Top.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Right.Color.SetColor(Color.Black);
            xlSheet.Cells[5, 1, 5, 13].Style.Border.Bottom.Color.SetColor(Color.Black);
            //END HEADER

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            foreach (NgayhuyhdViewModel vm in d)
            {
                xlSheet.Cells[iRowIndex, 1].Value = idem;
                TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 2].Value = vm.ngayhoadon.ToString("dd/MM/yyyy");
                TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 3].Value = vm.hdvat;
                TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 4].Value = vm.tenkhach;
                TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 5].Value = vm.diengiai;
                TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 6].Value = vm.sgtcode;
                TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 7].Value = vm.serial;
                TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 8].Value = vm.sotiennt;
                xlSheet.Cells[iRowIndex, 8].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 9].Value = vm.tienvnd;
                xlSheet.Cells[iRowIndex, 9].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 10].Value = vm.ppv;
                TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 11].Value = vm.tienppv;
                xlSheet.Cells[iRowIndex, 11].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 12].Value = vm.vat;
                TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                xlSheet.Cells[iRowIndex, 13].Value = vm.thuevat;
                xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0.0";
                TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                xlSheet.Cells[iRowIndex, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;
             
                iRowIndex += 1;
                idem += 1;
            }

            //dong tong
            xlSheet.Cells[iRowIndex, 1].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 2].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 3].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Left, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 4].Value = "Tổng cộng";
            TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 5].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 5, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 6].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 6, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 7].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 7, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 8].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 8, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 9].Formula = "=SUM(I6:I" + (5 + d.Count()) + ")";
            TrSetCellBorder(xlSheet, iRowIndex, 9, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 9].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 10].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 10, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 10].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 11].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 11, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 11].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 12].Value = "";
            TrSetCellBorder(xlSheet, iRowIndex, 12, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Center, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 12].Style.Border.Top.Style = ExcelBorderStyle.None;

            xlSheet.Cells[iRowIndex, 13].Formula = "=SUM(M6:M" + (5 + d.Count()) + ")";
            xlSheet.Cells[iRowIndex, 13].Style.Numberformat.Format = "#,##0";
            TrSetCellBorder(xlSheet, iRowIndex, 13, ExcelBorderStyle.Thin, ExcelHorizontalAlignment.Right, Color.Black, "Times New Roman", 10, FontStyle.Bold);
            xlSheet.Cells[iRowIndex, 13].Style.Border.Top.Style = ExcelBorderStyle.None;

           
            //end dong tong

            xlSheet.Cells.AutoFitColumns();
            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }
            string sFilename = "bc_dtgiam_ngayhuyhd" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );
        }
        #endregion

        #region Export to word
        public ActionResult Xemhoadon(string idhoadon)
        {
            var hd = _hoadonRepository.GetByTwoKey(idhoadon,HttpContext.Session.GetString("chinhanh"));
            var chinhanh = _dsdangkyhdRepository.getthongtinhd(HttpContext.Session.GetString("chinhanh"), HttpContext.Session.GetString("kyhieuhd"));
            var cthd = _cthdvatRepository.ListChitietHoadon(idhoadon, HttpContext.Session.GetString("chinhanh"));
            //DocX doc = null;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string fileName = webRootPath + @"\Wordtemp\hddt.docx";
            var doc = DocX.Load(fileName);
            doc.AddCustomProperty(new CustomProperty("mausohd",hd.mausohd));
            doc.AddCustomProperty(new CustomProperty("kyhieu", hd.kyhieu));
            doc.AddCustomProperty(new CustomProperty("sohd", hd.hdvat));
            doc.AddCustomProperty(new CustomProperty("ngayhd", hd.ngayin.HasValue? "(Date)Ngày "+hd.ngayin.Value.ToString("dd")+" tháng "+hd.ngayin.Value.ToString("MM")+" năm " +hd.ngayin.Value.ToString("yyyy"):" Ngày.....tháng.....năm..... "));
            doc.AddCustomProperty(new CustomProperty("tencongty", chinhanh.tencn));
            doc.AddCustomProperty(new CustomProperty("mstcty", chinhanh.masothue));
            doc.AddCustomProperty(new CustomProperty("diachicty", chinhanh.diachi));
            doc.AddCustomProperty(new CustomProperty("taikhoancty", ""));
            doc.AddCustomProperty(new CustomProperty("dienthoaicty",chinhanh.dienthoai));
            doc.AddCustomProperty(new CustomProperty("donvikhach", hd.tenkh));
            doc.AddCustomProperty(new CustomProperty("tenkhach", string.IsNullOrEmpty(hd.tenkhach)?"":hd.tenkhach));
            doc.AddCustomProperty(new CustomProperty("mstkhach", string.IsNullOrEmpty(hd.msthue)?"":hd.msthue));
            doc.AddCustomProperty(new CustomProperty("diachikhach", string.IsNullOrEmpty(hd.diachi)?"":hd.diachi.Trim()));

            double totalsotiennt = 0;
            double totaltienthue = 0;
            double totaltienppv = 0;
            double total = 0;
            var tbChitiet = doc.AddTable(1, 6);
            tbChitiet.SetWidths(new float[] { 50, 320, 100, 120,80,100 });

            tbChitiet.Rows[0].Cells[0].Paragraphs[0].Append("STT" + System.Environment.NewLine + " Order").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.center;
            tbChitiet.Rows[0].Cells[1].Paragraphs[0].Append("Tên hàng hoá, dịch vụ" + System.Environment.NewLine + " Description").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.center;
            tbChitiet.Rows[0].Cells[2].Paragraphs[0].Append("Số tiền" + System.Environment.NewLine + " Amount").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.center;
            tbChitiet.Rows[0].Cells[3].Paragraphs[0].Append("Tiền phí dịch vụ" + System.Environment.NewLine + " Service Chargers").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.center;
            tbChitiet.Rows[0].Cells[4].Paragraphs[0].Append("Thuế suất" + System.Environment.NewLine + " VAT Rate%").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.center;
            tbChitiet.Rows[0].Cells[5].Paragraphs[0].Append("Tiền thuế" + System.Environment.NewLine + " VAT Amount").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.center;
            foreach (var i in cthd)
            {
                var row = tbChitiet.InsertRow();
                double ptvat = Math.Round((i.vat / 100.0), 2);
                double tienthue = Math.Round((double)i.sotien *  ptvat, 0) ;
                double tienppv= Math.Round((double)i.sotien * ((double)i.ppv / 100.0), 0);
                row.Cells[0].Paragraphs[0].Append(i.sttdong==0?"": i.sttdong.ToString()).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.center;             
                row.Cells[1].Paragraphs[0].Append(i.diengiai).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.left;
                row.Cells[2].Paragraphs[0].Append(string.Format("{0:#,##0}", i.sotiennt)).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
                row.Cells[3].Paragraphs[0].Append(string.Format("{0:#,##0}",tienppv)).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
                row.Cells[4].Paragraphs[0].Append(i.vat.ToString()).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
                row.Cells[5].Paragraphs[0].Append(string.Format("{0:#,##0}",tienthue)).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;

                totalsotiennt += (double)i.sotiennt;
                totaltienthue += tienthue;
                totaltienppv += tienppv;
                total +=(double) i.sotiennt +tienthue+tienppv;
                //row.Cells[0].SetBorder(TableCellBorderType.Bottom, new Novacode.Border(BorderStyle.Tcbs_double, BorderSize.one, 1, Color.Transparent));
                //row.Cells[1].SetBorder(TableCellBorderType.Bottom, new Novacode.Border(BorderStyle.Tcbs_double, BorderSize.one, 1, Color.Transparent));
                //row.Cells[2].SetBorder(TableCellBorderType.Bottom, new Novacode.Border(BorderStyle.Tcbs_double, BorderSize.one, 1, Color.Transparent));
                //row.Cells[3].SetBorder(TableCellBorderType.Bottom, new Novacode.Border(BorderStyle.Tcbs_double, BorderSize.one, 1, Color.Transparent));
                //row.Cells[4].SetBorder(TableCellBorderType.Bottom, new Novacode.Border(BorderStyle.Tcbs_double, BorderSize.one, 1, Color.Transparent));
                //row.Cells[5].SetBorder(TableCellBorderType.Bottom, new Novacode.Border(BorderStyle.Tcbs_double, BorderSize.one, 1, Color.Transparent));
            }
            // set no border cho table
            Novacode.Border b = new Novacode.Border(Novacode.BorderStyle.Tcbs_none, BorderSize.seven, 0, Color.Transparent);
            tbChitiet.SetBorder(TableBorderType.InsideH, b);
            tbChitiet.SetBorder(TableBorderType.InsideV, b);
            tbChitiet.SetBorder(TableBorderType.Bottom, b);
            tbChitiet.SetBorder(TableBorderType.Top, b);
            tbChitiet.SetBorder(TableBorderType.Left, b);
            tbChitiet.SetBorder(TableBorderType.Right, b);

            // insert dong subtotal
            var rowsub = tbChitiet.InsertRow();
            rowsub.Cells[1].Paragraphs[0].Append("Cộng (Subtotal)").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
            rowsub.Cells[2].Paragraphs[0].Append(string.Format("{0:#,##0}", totalsotiennt)).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
            rowsub.Cells[3].Paragraphs[0].Append(string.Format("{0:#,##0}", totaltienppv)).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
            rowsub.Cells[5].Paragraphs[0].Append(string.Format("{0:#,##0}", totaltienthue)).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
            // insert dong Total
            var rowstotal = tbChitiet.InsertRow();
            rowstotal.Cells[1].Paragraphs[0].Append("Tổng cộng(Total)").Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
            rowstotal.Cells[2].Paragraphs[0].Append(string.Format("{0:#,##0}", total)).Color(Color.Black).Font("Times New Roman").FontSize(10).Alignment = Alignment.right;
           
            doc.InsertTable(tbChitiet);
            doc.InsertParagraph("Số tiền bằng chữ-Inwords: " + SoSangChu.DoiSoSangChu(Math.Round(total,0).ToString())).Color(Color.Black).Font("Times New Roman").FontSize(11).Alignment = Alignment.left ;

            MemoryStream stream = new MemoryStream();
            doc.SaveAs(stream);
            stream.Position = 0;
            //Download Word document in the browser
           
            return File(stream, "application/msword", "hoadon_" + DateTime.Now + ".docx");
        }


        #endregion


        #region listbox
        public void listDatatour(string selected = "")
        {
            try
            {
                List<SelectListItem> tour = new List<SelectListItem>();
                tour.Add(new SelectListItem { Text = " ", Value = "" });
                tour.Add(new SelectListItem { Text = "OB", Value = "OB" });
                tour.Add(new SelectListItem { Text = "ND", Value = "ND" });
                tour.Add(new SelectListItem { Text = "QLT", Value = "QLT" });
                ViewBag.tour = new SelectList(tour, "Text", "Value", selected);
            }
            catch { return; }
        }
        public void listNguontour(string selected = "")
        {
            try
            {
                List<Nguonhd> tour =  new List<Nguonhd>();
                tour=_nguonhdRepository.GetAll().Where(x => x.active == true).ToList();
                var a = new Nguonhd
                {
                    IdNguonhd = "",
                    Diengiai = ""
                };
                tour.Insert(0, a);
                ViewBag.tour = new SelectList(tour, "IdNguonhd", "IdNguonhd", selected);
            }
            catch { return; }
        }
        public void TrSetCellBorder(ExcelWorksheet xlSheet, int iRowIndex, int colIndex, ExcelBorderStyle excelBorderStyle, ExcelHorizontalAlignment excelHorizontalAlignment, Color borderColor, string fontName, int fontSize, FontStyle fontStyle)
        {
            xlSheet.Cells[iRowIndex, colIndex].Style.HorizontalAlignment = excelHorizontalAlignment;
            // Set Border
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Left.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Top.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Right.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Bottom.Style = excelBorderStyle;
            // Set màu ch Border
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Left.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Top.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Right.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Bottom.Color.SetColor(borderColor);

            // Set Font cho text  trong Range hiện tại                    
            xlSheet.Cells[iRowIndex, colIndex].Style.Font.SetFromFont(new System.Drawing.Font(fontName, fontSize, fontStyle));
        }
        #endregion
    }
}