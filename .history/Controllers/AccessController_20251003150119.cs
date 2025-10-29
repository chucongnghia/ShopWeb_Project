using Microsoft.AspNetCore.Mvc;
using ShopWeb_Project.Models;

namespace ShopWeb_Project.Controllers
{
    public class AccessController : Controller
    {
        ShopWebDbContext db = new ShopWebDbContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
              
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Login(TUser user)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var u = db.TUsers.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            if (u == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }

            HttpContext.Session.SetString("UserName", u.Username);

            // Admin redirect (username check can be replaced with role when available)
            if (string.Equals(u.Username, "nghia", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("DanhMucSanPham", "HomeAdmin", new { area = "admin" });
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }
    }
}
