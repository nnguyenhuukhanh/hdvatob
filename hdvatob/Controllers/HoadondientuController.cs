using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hdvatob.Data.Interfaces;
using hdvatob.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace hdvatob.Controllers
{
    public class HoadondientuController : BaseController
    {
        private readonly IHoadondientuRepository _hoadondientuRepository;
        private readonly IDsdangkyhdRepository _dsdangkyhdRepository;
        private readonly IHoadonRepository _hoadvatRepository;

        public HoadondientuController(IHoadondientuRepository hoadondientuRepository, IDsdangkyhdRepository dsdangkyhdRepository,
                                      IHoadonRepository hoadvatRepository)
        {
            _hoadondientuRepository = hoadondientuRepository;
            _dsdangkyhdRepository = dsdangkyhdRepository;
            _hoadvatRepository = hoadvatRepository;
        }
        // GET: Hoadondientu
        public ActionResult Index()
        {
            var hd = _hoadondientuRepository.dsChinhanhHoadondientu();
            return View(hd);
        }

     
        // GET: Hoadondientu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hoadondientu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChinhanhHoadondientu entity)
        {
            if (ModelState.IsValid)
            {
                int result = _hoadondientuRepository.themChinhanhHoadondientu(entity);
                if (result > 0)
                {
                    SetAlert("Thêm chi nhánh thành công", "success");
                }
                else
                {
                    SetAlert("Thêm chi nhánh không thành công", "error");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Hoadondientu/Edit/5
        public ActionResult Edit(string id)
        {
            HttpContext.Session.SetString("urlEditHoadondt", UriHelper.GetDisplayUrl(Request));
            var hd = _hoadondientuRepository.dsChinhanhHoadondientu().Find(x => x.machinhanh == id);
            //var dskyhd = _dsdangkyhdRepository.listDsDangkyhoadondientuByChinhanh(hd.maviettat);
            //if (dskyhd.Count() > 0)
            //{
            //    ViewBag.count = dskyhd.Count();
            //}
            //else
            //{
            //    ViewBag.count = 0;
            //}
            //ViewBag.dsdkhd = dskyhd;
            return View(hd);
        }

        // POST: Hoadondientu/Edit/5
        [HttpPost]
        public ActionResult Edit(ChinhanhHoadondientu entity)
        {
            //try
            //{
                if (entity.machinhanh == null)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    int result = _hoadondientuRepository.capnhatChinhanhHoadondientu(entity);
                    if (result > 0)
                    {
                        SetAlert("Cập nhật thông tin chi nhánh thành công", "success");
                    }
                    else
                    {
                        SetAlert("Cập nhật thông tin chi nhánh không thành công", "error");
                    }
                }

            return Redirect(HttpContext.Session.GetString("urlEditHoadondt"));
            //}
            //catch
            //{
            //    return View();
            //}
        }

       
        public IActionResult MacnExists(string machinhanh)
        {
            bool result = false;
            var u =  _hoadondientuRepository.dsChinhanhHoadondientu().Find(x => x.machinhanh == machinhanh);
            if (u == null)
                result = true;

            return Json(result);
        }

        public IActionResult listDsDangkyhoadon()
        {
            var ds = _dsdangkyhdRepository.listDsDangkyhoadondientu();            
            return View(ds);
        }

        [HttpGet]
        public IActionResult capnhatDsDangkyhoadon(int id)
        {
            var d = _dsdangkyhdRepository.getDkhdById(id);
            if (d == null)
            {
                return NotFound();
            }
            return View(d);
        }
        [HttpPost]
        public IActionResult capnhatDsDangkyhoadon(Dsdangkyhoadondientu entity)
        {
            if (ModelState.IsValid)
            {
                int n = _dsdangkyhdRepository.capnhatDangkyhoadondientu(entity);
                if (n > 0)
                {
                    SetAlert("Cập nhật thông tin đăng ký hoá đơn thành công", "success");
                }
                else
                {
                    SetAlert("Cập nhật thông tin đăng ký hoá đơn không thành công", "error");
                }
                //return Redirect(HttpContext.Session.GetString("urlEditHoadondt"));
            }
            return View(entity);
        }
        public IActionResult themDsDangkyhoadon ()//(string chinhanh)
        {
            var d = new Dsdangkyhoadondientu();
            
            d.sudungtungay = System.DateTime.Now;
            d.sudungdenngay = System.DateTime.Now;
            return View(d);
        }
        [HttpPost]
        public IActionResult themDsDangkyhoadon(Dsdangkyhoadondientu entity)
        {
            if (ModelState.IsValid)
            {
                entity.chinhanh = entity.chinhanh.ToUpper();
                int n = _dsdangkyhdRepository.themDangkyhoadondientu(entity);
                if (n > 0)
                {
                    SetAlert("Thêm đăng ký hoá đơn thành công", "success");
                }
                else
                {
                    SetAlert("Thêm đăng ký hoá đơn không thành công", "error");
                }
                
            }
            return View(entity);
        }
    }
}