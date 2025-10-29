using Microsoft.AspNetCore.Mvc;
using ShopWeb_Project.Models;

namespace ShopWeb_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly ShopWebDbContext _db = new ShopWebDbContext();

        [HttpGet]
        public IActionResult UserInfo()
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Access");
            }

            var user = _db.TUsers.FirstOrDefault(u => u.Username == username);
            var kh = _db.TKhachHangs.FirstOrDefault(k => k.Username == username) ?? new TKhachHang { Username = username };
            
            return View(kh);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserInfo(TKhachHang model)
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Access");
            }

            var existing = _db.TKhachHangs.FirstOrDefault(k => k.Username == username);
            if (existing == null)
            {
                // generate a simple key if missing
                model.MaKhanhHang = model.MaKhanhHang ?? Guid.NewGuid().ToString("N");
                model.Username = username;
                _db.TKhachHangs.Add(model);
            }
            else
            {
                existing.TenKhachHang = model.TenKhachHang;
                existing.NgaySinh = model.NgaySinh;
                existing.SoDienThoai = model.SoDienThoai;
                existing.DiaChi = model.DiaChi;
                existing.GhiChu = model.GhiChu;
                existing.AnhDaiDien = model.AnhDaiDien;
                existing.LoaiKhachHang = model.LoaiKhachHang;
                _db.Update(existing);
            }
            _db.SaveChanges();

            return RedirectToAction("UserInfo");
        }
    }
}


