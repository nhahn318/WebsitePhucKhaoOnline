using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using SinhVienModel = WebsitePhucKhao.Models.SinhVien;

namespace WebsitePhucKhao.Areas.Admin.Controllers {
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SinhVienController : Controller {
        private readonly ISinhVienRepository _sinhVienRepository;
        private readonly IKhoaRepository _khoaRepository;
        private readonly IChuyenNganhRepository _chuyenNganhRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SinhVienController(ISinhVienRepository sinhVienRepository, IKhoaRepository khoaRepository, IChuyenNganhRepository chuyenNganhRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _sinhVienRepository = sinhVienRepository;
            _khoaRepository = khoaRepository;
            _chuyenNganhRepository = chuyenNganhRepository;
            _context = context;
            _userManager = userManager;
        }

        // Hiển thị danh sách sinh viên
        public async Task<IActionResult> Index()
        {
            var sinhViens = await _sinhVienRepository.GetAllWithDetailsAsync();
            return View(sinhViens);
        }

        // Hiển thị form thêm sinh viên mới
        public async Task<IActionResult> Add()
        {
            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");

            // Chuyên ngành ban đầu rỗng, sẽ được cập nhật khi chọn Khoa
            ViewBag.ChuyenNganhList = new List<SelectListItem>();

            var lopList = await _context.Lops.ToListAsync(); // load tất cả lớp
            ViewBag.LopList = new SelectList(lopList, "MaLop", "TenLop");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SinhVienModel sinhVien)
        {
            if (!ModelState.IsValid)
            {
                var khoaList = await _khoaRepository.GetAllAsync();
                ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");
                ViewBag.ChuyenNganhList = new List<SelectListItem>();

                return View(sinhVien);
            }

            // Kiểm tra xem mã sinh viên đã tồn tại chưa
            var existingSinhVien = await _sinhVienRepository.GetByIdAsync(sinhVien.MaSinhVien);
            if (existingSinhVien != null)
            {
                ModelState.AddModelError("MaSinhVien", "Mã sinh viên đã tồn tại!");
                return View(sinhVien);
            }

            // Kiểm tra xem email đã tồn tại trong bảng AspNetUsers chưa
            var existingUser = await _userManager.FindByEmailAsync(sinhVien.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email này đã được dùng cho một tài khoản khác!");
                return View(sinhVien);
            }

            // Đặt mật khẩu mặc định là mã sinh viên
            sinhVien.MatKhau = sinhVien.MaSinhVien.ToString();

            // Bước 1: Thêm vào bảng SinhViens
            await _sinhVienRepository.AddAsync(sinhVien);

            // Bước 2: Tạo ApplicationUser tương ứng
            var appUser = new ApplicationUser
            {
                UserName = sinhVien.Email,
                Email = sinhVien.Email,
                MaSinhVien = sinhVien.MaSinhVien
            };

            var createResult = await _userManager.CreateAsync(appUser, sinhVien.MaSinhVien.ToString());

            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(sinhVien);
            }

            // Bước 3: Gán role "SinhVien" nếu chưa có
            if (!await _userManager.IsInRoleAsync(appUser, "SinhVien"))
            {
                await _userManager.AddToRoleAsync(appUser, "SinhVien");
            }

            return RedirectToAction(nameof(Index));
        }


        // Hiển thị thông tin chi tiết sinh viên
        public async Task<IActionResult> Details(long id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        public async Task<IActionResult> Update(long id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }

            // Lấy danh sách Khoa từ Database
            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa", sinhVien.MaKhoa);

            // Lấy danh sách Chuyên ngành theo Khoa của sinh viên
            var chuyenNganhList = await _chuyenNganhRepository.GetByKhoaIdAsync(sinhVien.MaKhoa);
            ViewBag.ChuyenNganhList = new SelectList(chuyenNganhList, "MaChuyenNganh", "TenChuyenNganh", sinhVien.MaChuyenNganh);

            return View(sinhVien);
        }


        [HttpPost]
        public async Task<IActionResult> Update(long id, SinhVienModel sinhVien)
        {
            ModelState.Remove("MatKhau"); // bỏ qua mật khẩu khi cập nhật nếu bạn không gửi lên
            if (ModelState.IsValid)
            {
                var existingSinhVien = await _sinhVienRepository.GetByIdAsync(id);
                if (existingSinhVien == null)
                {
                    return NotFound();
                }

                existingSinhVien.HoTen = sinhVien.HoTen;
                existingSinhVien.Email = sinhVien.Email;
                existingSinhVien.SoDienThoai = sinhVien.SoDienThoai;
                existingSinhVien.Khoa = sinhVien.Khoa;
                existingSinhVien.ChuyenNganh = sinhVien.ChuyenNganh;
                existingSinhVien.Lop = sinhVien.Lop;

                await _sinhVienRepository.UpdateAsync(existingSinhVien);
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi, hiển thị lỗi ra View
            return View(sinhVien);
        }

        // Hiển thị form xác nhận xóa sinh viên
        public async Task<IActionResult> Delete(long id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            // Bước 1: Xóa tất cả user liên quan trong AspNetUsers trước
            var users = _context.Users.Where(u => u.MaSinhVien == id).ToList();
            if (users.Any())
            {
                _context.Users.RemoveRange(users);
                await _context.SaveChangesAsync(); // Lưu thay đổi trước khi tiếp tục
            }

            // Bước 2: Xóa sinh viên sau khi đã xóa user
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien != null)
            {
                await _sinhVienRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetChuyenNganhsByKhoa(string maKhoa)
        {
            if (string.IsNullOrEmpty(maKhoa)) return Json(new List<object>());

            var chuyenNganhs = await _chuyenNganhRepository.GetByKhoaIdAsync(int.Parse(maKhoa));

            var result = chuyenNganhs.Select(cn => new {
                maChuyenNganh = cn.MaChuyenNganh,
                tenChuyenNganh = cn.TenChuyenNganh
            });

            return Json(result);
        }


        public async Task<IActionResult> GetLopsByKhoa(string maKhoa)
        {
            if (string.IsNullOrEmpty(maKhoa)) return Json(new List<object>());

            var lops = await _context.Lops
                .Where(l => l.MaKhoa == int.Parse(maKhoa))
                .Select(l => new {
                    maLop = l.MaLop,
                    tenLop = l.TenLop
                }).ToListAsync();

            return Json(lops);
        }


        public async Task<IActionResult> CapNhatThongTin()
        {
            var userEmail = User.Identity?.Name; // Kiểm tra null trước khi sử dụng
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(); // Nếu không có email, trả về lỗi 401
            } // Lấy email của người đăng nhập
            var sinhVien = await _sinhVienRepository.GetByEmailAsync(userEmail);

            if (sinhVien == null)
            {
                return NotFound();
            }
            // Lấy danh sách Khoa
            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa", sinhVien.MaKhoa);

            // Lấy danh sách Chuyên ngành theo Khoa
            var chuyenNganhList = await _chuyenNganhRepository.GetByKhoaIdAsync(sinhVien.MaKhoa);
            ViewBag.ChuyenNganhList = new SelectList(chuyenNganhList, "MaChuyenNganh", "TenChuyenNganh", sinhVien.MaChuyenNganh);

            // Lấy danh sách Lớp theo Khoa
            var lopList = await _context.Lops.Where(l => l.MaKhoa == sinhVien.MaKhoa).ToListAsync();
            ViewBag.LopList = new SelectList(lopList, "MaLop", "TenLop", sinhVien.MaLop);

            return View(sinhVien);
        }
        [HttpPost]
        public async Task<IActionResult> CapNhatThongTin(SinhVienModel sinhVien)
        {
            if (!ModelState.IsValid)
            {
                return View(sinhVien);
            }

            var userEmail = User.Identity?.Name; // Kiểm tra null trước khi sử dụng
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(); // Nếu không có email, trả về lỗi 401
            }
            var existingSinhVien = await _sinhVienRepository.GetByEmailAsync(userEmail);

            if (existingSinhVien == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin
            existingSinhVien.HoTen = sinhVien.HoTen;
            existingSinhVien.Email = sinhVien.Email;
            existingSinhVien.SoDienThoai = sinhVien.SoDienThoai;
            existingSinhVien.MaKhoa = sinhVien.MaKhoa;
            existingSinhVien.MaChuyenNganh = sinhVien.MaChuyenNganh;
            existingSinhVien.MaLop = sinhVien.MaLop;

            await _sinhVienRepository.UpdateAsync(existingSinhVien);

            TempData["Message"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("CapNhatThongTin");
        }



    }
}