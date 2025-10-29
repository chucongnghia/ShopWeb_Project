using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopWeb_Project.Models;
using ShopWeb_Project.Models.ProductModels;

namespace ShopWeb_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        ShopWebDbContext db = new ShopWebDbContext();
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            var sanPham=(from p in db.TDanhMucSps select new Product
            {
                MaSp=p.MaSp,
                TenSp=p.TenSp,
                MaLoai=p.MaLoai,
                AnhDaiDien=p.AnhDaiDien,
                GiaNhoNhat=p.GiaNhoNhat,

            }).ToList();
            return sanPham;
        }
        [HttpGet("{maloai}")]
        public IEnumerable<Product> GetProductByCategory(string maloai)
        {
            var sanPham = (from p in db.TDanhMucSps where p.MaLoai == maloai
                           select new Product
                           {
                               MaSp=p.MaSp,
                               TenSp=p.TenSp,
                               MaLoai=p.MaLoai,
                               AnhDaiDien=p.AnhDaiDien,
                               GiaNhoNhat=p.GiaNhoNhat,

                           }).ToList();
            return sanPham;
        }
    }
}
