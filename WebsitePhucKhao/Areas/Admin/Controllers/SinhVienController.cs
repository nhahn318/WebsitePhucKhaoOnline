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

        // Hiển thị form thêm sinh viên
        public IActionResult Add()
        {
            return View();
        }

        // Xử lý thêm sinh viên
        [HttpPost]
        public async Task<IActionResult> Add(SinhVienModel sinhVien)
        {
            if (ModelState.IsValid)
            {
                await _sinhVienRepository.AddAsync(sinhVien);
                return RedirectToAction(nameof(Index));
            }
            return View(sinhVien);
        }

        // Hiển thị chi tiết sinh viên
        public async Task<IActionResult> Details(long id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        // Hiển thị form cập nhật sinh viên
        public async Task<IActionResult> Update(long id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        // Xử lý cập nhật sinh viên
        [HttpPost]
        public async Task<IActionResult> Update(long id, SinhVienModel sinhVien)
        {
            if (id != sinhVien.MaSinhVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _sinhVienRepository.UpdateAsync(sinhVien);
                return RedirectToAction(nameof(Index));
            }
            return View(sinhVien);
        }

        // Xử lý xóa sinh viên
        public async Task<IActionResult> Delete(long id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            await _sinhVienRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetChuyenNganhsByKhoa(int maKhoa)
        {
            var chuyenNganhs = await _chuyenNganhRepository.GetByKhoaIdAsync(maKhoa);
            return Json(chuyenNganhs);
        }


        public async Task<IActionResult> GetLopsByKhoa(int maKhoa)
        {
            var lops = await _context.Lops
                                     .Where(l => l.MaKhoa == maKhoa)
                                     .Select(l => new { l.MaLop, l.TenLop })
                                     .ToListAsync();
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
