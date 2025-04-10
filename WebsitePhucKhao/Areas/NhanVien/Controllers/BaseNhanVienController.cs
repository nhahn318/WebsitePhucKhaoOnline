using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Areas.NhanVien.Controllers
{
    [Area("NhanVien")]
    [Authorize(Roles = "Admin,NhanVien")]
    public class BaseNhanVienController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BaseNhanVienController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected async Task<string> GetLayoutBasedOnRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return "~/Areas/NhanVien/Views/Shared/_LayoutNhanVien.cshtml";
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            return isAdmin 
                ? "~/Areas/Admin/Views/Shared/_LayoutAdmin.cshtml" 
                : "~/Areas/NhanVien/Views/Shared/_LayoutNhanVien.cshtml";
        }

        protected async Task<IActionResult> ViewWithLayout(string viewName, object model = null)
        {
            ViewData["Layout"] = await GetLayoutBasedOnRole();
            return View(viewName, model);
        }
    }
} 