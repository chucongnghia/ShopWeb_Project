using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWeb_Project.Models;
using ShopWeb_Project.Models.Authentication;
using ShopWeb_Project.ViewModels;
using X.PagedList;


namespace ShopWeb_Project.Controllers
{
    public class HomeController : Controller

    {
        ShopWebDbContext db = new ShopWebDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //[Authentication]
        public IActionResult Index(int? page, string? search)
        {
            int pageSize = 8;
            int pageNumber=page==null||page<0?1:page.Value;
            var query = db.TDanhMucSps.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim();
                query = query.Where(x => x.TenSp.Contains(keyword) || x.MaSp.Contains(keyword));
            }

            var listSanpham=query.OrderBy(x=>x.TenSp);
            PagedList<TDanhMucSp>lst=new PagedList<TDanhMucSp>(listSanpham,pageNumber,pageSize);

            return View(lst);
        }
        //[Authentication]
        public IActionResult SanPhamTheoLoai(String maloai,int? page)
        {
            int pageSize = 8;
            int pageNumber = page==null||page<0 ? 1 : page.Value;
            var listSanpham = db.TDanhMucSps.AsNoTracking().Where(x=>x.MaLoai==maloai).OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(listSanpham, pageNumber, pageSize);
            ViewBag.maloai = maloai;
            return View(lst);
        }
        public IActionResult ChiTietSanPham(string maSp)
        {
            var sanPham=db.TDanhMucSps.SingleOrDefault(x=>x.MaSp==maSp);
            var anhSanPham=db.TAnhSps.Where(x=>x.MaSp==maSp).ToList();
            ViewBag.anhSanPham = anhSanPham;
            return View(sanPham);   
        }
        
        public IActionResult ProductDetail(string maSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp==maSp);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp==maSp).ToList();
            var homeProductDetailViewModel = new HomeProductDetailViewModel { danhmucSp =sanPham, anhSps =anhSanPham};
            return View(homeProductDetailViewModel);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
