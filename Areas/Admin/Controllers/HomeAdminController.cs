using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Project.Models;
using X.PagedList;

namespace ShopWeb_Project.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]

    public class HomeAdminController : Controller
    {
        ShopWebDbContext db = new ShopWebDbContext();
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {

            return View();
        }
        [Route("danhmucsanpham")]
        public IActionResult DanhMucSanPham(int? page, string? search)
        {
            int pageSize = 8;
            int pageNumber = page==null||page<0 ? 1 : page.Value;
            var query = db.TDanhMucSps.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim();
                query = query.Where(x => x.TenSp.Contains(keyword) || x.MaSp.Contains(keyword));
            }

            var listSanpham = query.OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(listSanpham, pageNumber, pageSize);

            return View(lst);
        }
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.MaChatLieu=new SelectList(db.TChatLieus.ToList(),"MaChatLieu","ChatLieu");
            ViewBag.MaHangSx=new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx=new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai=new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt=new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(TDanhMucSp sp)
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sp);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sp);
        }

        [Route("ChinhSuaSanPham")]
        [HttpGet]
        public IActionResult ChinhSuaSanPham(string sanpham)
        {
            ViewBag.MaChatLieu=new SelectList(db.TChatLieus.ToList(),"MaChatLieu","ChatLieu");
            ViewBag.MaHangSx=new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx=new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai=new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt=new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            var sp = db.TDanhMucSps.Find(sanpham);
            return View(sanpham);
        }
        [Route("ChinhSuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChinhSuaSanPham(TDanhMucSp sp)
        {
            if (ModelState.IsValid)
            {
                db.Update(sp);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham","HomeAdmin");
            }
            return View(sp);
        }

        
        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSanPham)
        {
            TempData["Message"] = "";
            var chitietSp=db.TChiTietSanPhams.Where(ct => ct.MaSp == maSanPham).ToList();
            if(chitietSp.Count > 0)
            {
                TempData["Message"] = "Sản phẩm này đã có trong chi tiết đơn hàng, không thể xóa!";
                return RedirectToAction("DanhMucSanPham","HomeAdmin");
            }
            var anhSanPham=db.TAnhSps.Where(a => a.MaSp==maSanPham);
            if (anhSanPham.Any())
            {
                db.RemoveRange(anhSanPham);
            }
            db.Remove(db.TDanhMucSps.Find(maSanPham));
            db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa thành công!";
            return RedirectToAction("DanhMucSanPham","HomeAdmin");
        }
        // add Details action
        [Route("ChiTietSanPham")]
        [HttpGet]
        public IActionResult ChiTietSP(string maSanPham)
        {
            var sp = db.TDanhMucSps.Include(p => p.MaChatLieuNavigation)
                                   .Include(p => p.MaHangSxNavigation)
                                   .Include(p => p.MaNuocSxNavigation)
                                   .Include(p => p.MaLoaiNavigation)
                                   .Include(p => p.MaDtNavigation)
                                   .FirstOrDefault(p => p.MaSp == maSanPham);
            if (sp == null)
            {
                return NotFound();
            }
            return View(sp);
        }
        [Route("ChiTietSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChiTietSP(TDanhMucSp sp)
        {
            if (ModelState.IsValid)
            {
                db.Update(sp);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham","HomeAdmin");
            }
            return View(sp);
        }
        
        
        
    }
}
