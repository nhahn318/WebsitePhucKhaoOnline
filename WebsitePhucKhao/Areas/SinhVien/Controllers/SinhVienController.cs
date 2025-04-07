using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Enums;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using SinhVienModel = WebsitePhucKhao.Models.SinhVien;

namespace WebsitePhucKhao.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    [Authorize(Roles = "SinhVien")]
    public class SinhVienController : Controller
    {
        private readonly ISinhVienRepository _sinhVienRepository;
        private readonly IKhoaRepository _khoaRepository;
        private readonly IChuyenNganhRepository _chuyenNganhRepository;
        private readonly ApplicationDbContext _context;

        public SinhVienController(
            ISinhVienRepository sinhVienRepository,
            IKhoaRepository khoaRepository,
            IChuyenNganhRepository chuyenNganhRepository,
            ApplicationDbContext context)
        {
            _sinhVienRepository = sinhVienRepository;
            _khoaRepository = khoaRepository;
            _chuyenNganhRepository = chuyenNganhRepository;
            _context = context;
        }
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Lấy username đang đăng nhập
                var userName = User.Identity?.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Không xác định được người dùng.");
                }

                // Truy vấn ApplicationUser
                var appUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == userName);

                if (appUser == null || appUser.MaSinhVien == null)
                {
                    return NotFound("Không tìm thấy thông tin sinh viên.");
                }

                // Truy vấn sinh viên theo MaSinhVien
                var student = await _context.SinhViens
                    .FirstOrDefaultAsync(s => s.MaSinhVien == appUser.MaSinhVien);

                if (student == null)
                {
                    return NotFound("Không tìm thấy hồ sơ sinh viên.");
                }


                // Lấy tất cả đơn phúc khảo của sinh viên
                var requests = await _context.DonPhucKhaos
                    .Where(r => r.MaSinhVien == student.MaSinhVien)
                    .OrderByDescending(r => r.NgayGui)
                    .ToListAsync();


                // Thống kê đơn theo enum
                ViewBag.TotalRequests = requests.Count;
                ViewBag.PendingRequests = requests.Count(r =>
                    r.TrangThai == TrangThaiPhucKhao.ChoXacNhan ||
                    r.TrangThai == TrangThaiPhucKhao.ChoCham);

                ViewBag.ProcessedRequests = requests.Count(r =>
                    r.TrangThai == TrangThaiPhucKhao.DaDuyet ||
                    r.TrangThai == TrangThaiPhucKhao.DaCham ||
                    r.TrangThai == TrangThaiPhucKhao.TuChoi);

                // Lấy 5 đơn mới nhất
                ViewBag.RecentRequests = requests.Take(5).ToList();

                // Truy vấn lịch thi cho sinh viên theo MaSinhVien
                var lichThiList = await _context.DonPhucKhaos
    .Where(r => r.MaSinhVien == student.MaSinhVien)  // Tìm tất cả đơn phúc khảo của sinh viên
    .Select(r => r.LichThi)  // Lấy lịch thi từ các đơn phúc khảo
    .OrderBy(l => l.NgayThi)
    .ToListAsync();


                // Gửi lịch thi vào ViewBag
                ViewBag.LichThi = lichThiList;

                // Thêm ViewBag.TenHocKy nếu có
                var hocKyHienTai = await _context.HocKys
                    .OrderByDescending(h => h.NgayBatDauPhucKhao)
                    .FirstOrDefaultAsync();

                ViewBag.TenHocKy = hocKyHienTai?.TenHocKy;

                return View(student);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine("Lỗi khi load dashboard: " + ex.Message);
                return View("Error");
            }
        }
        public async Task<IActionResult> CapNhatThongTin()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var sinhVien = await _sinhVienRepository.GetByEmailAsync(userEmail);
            if (sinhVien == null)
            {
                return NotFound();
            }

            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa", sinhVien.MaKhoa);

            var chuyenNganhList = await _chuyenNganhRepository.GetByKhoaIdAsync(sinhVien.MaKhoa);
            ViewBag.ChuyenNganhList = new SelectList(chuyenNganhList, "MaChuyenNganh", "TenChuyenNganh", sinhVien.MaChuyenNganh);

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

            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var existingSinhVien = await _sinhVienRepository.GetByEmailAsync(userEmail);
            if (existingSinhVien == null)
            {
                return NotFound();
            }

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
