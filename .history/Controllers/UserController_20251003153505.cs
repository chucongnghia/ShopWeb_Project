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
            ViewBag.Money = user?.Money ?? 0;
            return View(kh);
        }

        [HttpGet]
        public IActionResult TopUp()
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Access");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TopUp(decimal amount)
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Access");
            }
            if (amount <= 0)
            {
                ModelState.AddModelError("amount", "Amount must be greater than 0");
                return View();
            }
            var user = _db.TUsers.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Login", "Access");
            }
            user.Money = (user.Money ?? 0) + (int)amount;
            _db.Update(user);
            _db.SaveChanges();
            return RedirectToAction("UserInfo");
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


