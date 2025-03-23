using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebsitePhucKhao.Controllers {
    public class GiangVienController : Controller {
        private readonly IGiangVienRepository _giangVienRepository;
        private readonly IKhoaRepository _khoaRepository;

        public GiangVienController(IGiangVienRepository giangVienRepository, IKhoaRepository khoaRepository)
        {
            _giangVienRepository = giangVienRepository;
            _khoaRepository = khoaRepository;
        }

        // Hiển thị danh sách giảng viên
        public async Task<IActionResult> Index()
        {
            var giangViens = await _giangVienRepository.GetAllWithDetailsAsync();
            return View(giangViens);
        }

        // Hiển thị form thêm giảng viên mới
        public async Task<IActionResult> Add()
        {
            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GiangVien giangVien)
        {
            if (!ModelState.IsValid)
            {
                var khoaList = await _khoaRepository.GetAllAsync();
                ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");
                return View(giangVien);
            }

            await _giangVienRepository.AddAsync(giangVien);
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
        public async Task<IActionResult> Update(long id, GiangVien giangVien)
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

        [HttpPost, ActionName("DeleteConfirmed")]
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
