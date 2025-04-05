using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using SinhVienModel = WebsitePhucKhao.Models.SinhVien;

namespace WebsitePhucKhao.Areas.SinhVien.Controllers {
    [Area("SinhVien")]
    [Authorize(Roles = "SinhVien")]
    public class SinhVienController : Controller {
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
