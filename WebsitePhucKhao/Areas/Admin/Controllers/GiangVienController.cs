using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using Microsoft.AspNetCore.Identity;
using GiangVienModel = WebsitePhucKhao.Models.GiangVien;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebsitePhucKhao.Areas.Admin.Controllers {
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GiangVienController : Controller
    {
        private readonly IGiangVienRepository _giangVienRepository;
        private readonly IKhoaRepository _khoaRepository;
        private readonly IDonPhucKhaoChiTietRepository _DonPhucKhaoChiTietRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public GiangVienController(IGiangVienRepository giangVienRepository, IKhoaRepository khoaRepository, IDonPhucKhaoChiTietRepository donPhucKhaoChiTietRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _giangVienRepository = giangVienRepository;
            _khoaRepository = khoaRepository;
            _DonPhucKhaoChiTietRepository = donPhucKhaoChiTietRepository;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var giangViens = await _giangVienRepository.GetAllAsync();
            return View(giangViens);
        }

        public async Task<IActionResult> Add()
        {
            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GiangVienModel giangVien)
        {
            if (!ModelState.IsValid)
            {
                return View(giangVien);
            }

            // Thêm vào bảng GiangVien
            await _giangVienRepository.AddAsync(giangVien);

            // Tạo tài khoản ứng với GiangVien
            var appUser = new ApplicationUser
            {
                UserName = giangVien.Email,
                Email = giangVien.Email,
                MaGiangVien = giangVien.MaGiangVien
            };

            var createResult = await _userManager.CreateAsync(appUser, giangVien.MaGiangVien.ToString()); // dùng mã giảng viên làm password

            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var khoaList = await _khoaRepository.GetAllAsync();
                ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");
                return View(giangVien);
            }

            // Gán vai trò GiangVien
            await _userManager.AddToRoleAsync(appUser, "GiangVien");

            return RedirectToAction(nameof(Index));
        }


        // Hiển thị thông tin chi tiết giảng viên
        public async Task<IActionResult> Details(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }
            return View(giangVien);
        }

        public async Task<IActionResult> Update(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }

            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa", giangVien.MaKhoa);

            return View(giangVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(long id, GiangVienModel giangVien)
        {
            if (ModelState.IsValid)
            {
                var existingGiangVien = await _giangVienRepository.GetByIdAsync(id);
                if (existingGiangVien == null)
                {
                    return NotFound();
                }

                existingGiangVien.HoTen = giangVien.HoTen;
                existingGiangVien.Email = giangVien.Email;
                existingGiangVien.SoDienThoai = giangVien.SoDienThoai;
                existingGiangVien.MaKhoa = giangVien.MaKhoa;

                await _giangVienRepository.UpdateAsync(existingGiangVien);
                return RedirectToAction(nameof(Index));
            }

            return View(giangVien);
        }

        // Hiển thị form xác nhận xóa giảng viên
        public async Task<IActionResult> Delete(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }
            return View(giangVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien != null)
            {
                await _giangVienRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
