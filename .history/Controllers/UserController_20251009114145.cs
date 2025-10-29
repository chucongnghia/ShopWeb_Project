using Microsoft.AspNetCore.Mvc;
using ShopWeb_Project.Models;

namespace ShopWeb_Project.Controllers
{
    public class UserController : Controller
    {
        private readonly ShopWebDbContext _db;

        public UserController(ShopWebDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetUserInfo()
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return Json(new { success = false, message = "Chưa đăng nhập" });
            }

            try
            {
                var kh = _db.TKhachHangs.FirstOrDefault(k => k.Username == username);
                if (kh == null)
                {
                    // Tạo mới nếu chưa có
                    kh = new TKhachHang 
                    { 
                        Username = username,
                        MaKhanhHang = GenerateCustomerCode()
                    };
                    _db.TKhachHangs.Add(kh);
                    _db.SaveChanges();
                }

                return Json(new { 
                    success = true, 
                    data = new {
                        maKhanhHang = kh.MaKhanhHang,
                        username = kh.Username,
                        tenKhachHang = kh.TenKhachHang,
                        ngaySinh = kh.NgaySinh?.ToString("yyyy-MM-dd"),
                        soDienThoai = kh.SoDienThoai,
                        loaiKhachHang = kh.LoaiKhachHang,
                        diaChi = kh.DiaChi,
                        ghiChu = kh.GhiChu
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SaveUserInfo(string TenKhachHang, string NgaySinh, string SoDienThoai, 
            string LoaiKhachHang, string DiaChi, string GhiChu)
        {
            var username = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(username))
            {
                return Json(new { success = false, message = "Chưa đăng nhập" });
            }

            try
            {
                var existing = _db.TKhachHangs.FirstOrDefault(k => k.Username == username);
                
                if (existing == null)
                {
                    // Tạo mới
                    existing = new TKhachHang 
                    { 
                        Username = username,
                        MaKhanhHang = GenerateCustomerCode()
                    };
                    _db.TKhachHangs.Add(existing);
                }

                // Cập nhật thông tin
                existing.TenKhachHang = TenKhachHang;
                existing.SoDienThoai = SoDienThoai;
                existing.DiaChi = DiaChi;
                existing.GhiChu = GhiChu;
                
                if (!string.IsNullOrEmpty(NgaySinh) && DateOnly.TryParse(NgaySinh, out var ngaySinh))
                {
                    existing.NgaySinh = ngaySinh;
                }
                
                if (!string.IsNullOrEmpty(LoaiKhachHang) && byte.TryParse(LoaiKhachHang, out var loaiKhachHang))
                {
                    existing.LoaiKhachHang = loaiKhachHang;
                }

                _db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string GenerateCustomerCode()
        {
            var count = _db.TKhachHangs.Count() + 1;
            return $"KH{count:D6}";
        }
    }
}


