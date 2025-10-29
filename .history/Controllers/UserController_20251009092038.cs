using Microsoft.AspNetCore.Mvc;
using ShopWeb_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace ShopWeb_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly ShopWebDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public UserController(ShopWebDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

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
        public async Task<IActionResult> UserInfo(TKhachHang model, IFormFile avatarFile)
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Access");
            }

            // Validation
            if (string.IsNullOrWhiteSpace(model.TenKhachHang))
            {
                ModelState.AddModelError("TenKhachHang", "Tên khách hàng không được để trống");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var existing = _db.TKhachHangs.FirstOrDefault(k => k.Username == username);
                
                // Xử lý upload ảnh đại diện
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = $"{username}_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(avatarFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }

                    model.AnhDaiDien = $"/uploads/avatars/{fileName}";
                }

                if (existing == null)
                {
                    // Tạo mới khách hàng
                    model.MaKhanhHang = model.MaKhanhHang ?? GenerateCustomerCode();
                    model.Username = username;
                    _db.TKhachHangs.Add(model);
                }
                else
                {
                    // Cập nhật thông tin khách hàng
                    existing.TenKhachHang = model.TenKhachHang;
                    existing.NgaySinh = model.NgaySinh;
                    existing.SoDienThoai = model.SoDienThoai;
                    existing.DiaChi = model.DiaChi;
                    existing.GhiChu = model.GhiChu;
                    existing.LoaiKhachHang = model.LoaiKhachHang;
                    
                    // Chỉ cập nhật ảnh nếu có upload file mới
                    if (avatarFile != null && avatarFile.Length > 0)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(existing.AnhDaiDien))
                        {
                            var oldImagePath = Path.Combine(_environment.WebRootPath, existing.AnhDaiDien.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        existing.AnhDaiDien = model.AnhDaiDien;
                    }
                    
                    _db.Update(existing);
                }

                await _db.SaveChangesAsync();
                TempData["Message"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("UserInfo");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                return View(model);
            }
        }

        private string GenerateCustomerCode()
        {
            var count = _db.TKhachHangs.Count() + 1;
            return $"KH{count:D6}";
        }
    }
}


