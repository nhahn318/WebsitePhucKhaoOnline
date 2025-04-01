using Microsoft.AspNetCore.Mvc;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.ViewModels;
using WebsitePhucKhao.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebsitePhucKhao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NhanVienPhongDaoTaoController : Controller
    {
        private readonly INhanVienPhongDaoTaoRepository _nhanVienRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NhanVienPhongDaoTaoController(INhanVienPhongDaoTaoRepository nhanVienRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _nhanVienRepository = nhanVienRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var nhanViens = await _nhanVienRepository.GetAllAsync();
            return View(nhanViens);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NhanVienPhongDaoTao nhanVien)
        {
            if (ModelState.IsValid)
            {
                // Bước 1: Thêm nhân viên vào bảng NhanVienPhongDaoTao
                await _nhanVienRepository.AddAsync(nhanVien);

                // Bước 2: Tạo tài khoản ứng với nhân viên
                var user = new ApplicationUser
                {
                    UserName = nhanVien.Email,
                    Email = nhanVien.Email,
                    MaNhanVienPhongDaoTao = nhanVien.MaNhanVienPhongDaoTao // nhớ có property này trong ApplicationUser
                };

                var password = nhanVien.MaNhanVienPhongDaoTao.ToString(); // Mật khẩu mặc định
                var createResult = await _userManager.CreateAsync(user, password);

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(nhanVien); // Có lỗi, quay lại View
                }

                // Bước 3: Gán role "NhanVien"
                if (!await _userManager.IsInRoleAsync(user, "NhanVien"))
                {
                    await _userManager.AddToRoleAsync(user, "NhanVien");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nhanVien);
        }


        public async Task<IActionResult> Update(int id)
        {
            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, NhanVienPhongDaoTao nhanVien)
        {
            if (ModelState.IsValid)
            {
                await _nhanVienRepository.UpdateAsync(nhanVien);
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }

            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }
        public async Task<IActionResult> Delete(long id)
        {
            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            // Bước 1: Xóa tài khoản liên kết nếu có
            var users = await _context.Users
                .Where(u => u.MaNhanVienPhongDaoTao == id)
                .ToListAsync();

            if (users.Any())
            {
                _context.Users.RemoveRange(users);
                await _context.SaveChangesAsync(); 
            }

            // Bước 2: Xóa nhân viên khỏi bảng NhanVienPhongDaoTaos
            var nhanVien = await _context.NhanVienPhongDaoTaos.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanVienPhongDaoTaos.Remove(nhanVien);
                await _context.SaveChangesAsync(); // Lưu sau khi xóa nhân viên
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChiTietSauPhucKhao(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.ChiTietPhucKhao)
                .Include(d => d.HinhAnhBaiThis)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null) return NotFound();

            var chiTiet = don.ChiTietPhucKhao;

            var vm = new DonPhucKhaoSauKhiChamViewModel
            {
                MaDon = don.MaDon,
                NgayGui = don.NgayGui,
                TrangThai = don.TrangThai.ToFriendlyString(),
                DiemHienTai = don.DiemHienTai,
                DiemSauPhucKhao = chiTiet?.DiemSauPhucKhao,
                NhanXet = chiTiet?.NhanXet,
                BaiGiaiTayUrl = chiTiet?.BaiGiaiTayUrl,
                DanhSachAnh = don.HinhAnhBaiThis?.ToList() ?? new List<HinhAnhBaiThi>()
            };

            return View(vm);
        }

    }
}

