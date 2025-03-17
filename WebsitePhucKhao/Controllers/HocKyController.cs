using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Threading.Tasks;

namespace WebsitePhucKhao.Controllers {
    public class HocKyController : Controller {
        private readonly IHocKyRepository _hocKyRepository;
        private readonly INamHocRepository _namHocRepository;

        public HocKyController(IHocKyRepository hocKyRepository, INamHocRepository namHocRepository)
        {
            _hocKyRepository = hocKyRepository;
            _namHocRepository = namHocRepository;
        }

        // Hiển thị danh sách học kỳ
        public async Task<IActionResult> Index()
        {
            var hocKys = await _hocKyRepository.GetAllAsync(); // Load cả thông tin năm học
            return View(hocKys);
        }

        // Hiển thị form thêm học kỳ
        public async Task<IActionResult> Add()
        {
            var namHocs = await _namHocRepository.GetAllAsync();
            ViewBag.NamHocList = new SelectList(namHocs, "MaNamHoc", "TenNamHoc");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(HocKy hocKy)
        {
            if (!ModelState.IsValid)
            {
                var namHocs = await _namHocRepository.GetAllAsync();
                ViewBag.NamHocList = new SelectList(namHocs, "MaNamHoc", "TenNamHoc");
                return View(hocKy);
            }

            await _hocKyRepository.AddAsync(hocKy);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị form cập nhật học kỳ
        public async Task<IActionResult> Update(int id)
        {
            var hocKy = await _hocKyRepository.GetByIdAsync(id);
            if (hocKy == null)
            {
                return NotFound();
            }

            var namHocs = await _namHocRepository.GetAllAsync();
            ViewBag.NamHocList = new SelectList(namHocs, "MaNamHoc", "TenNamHoc", hocKy.MaNamHoc);

            return View(hocKy);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, HocKy hocKy)
        {
            if (id != hocKy.MaHocKy)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var namHocs = await _namHocRepository.GetAllAsync();
                ViewBag.NamHocList = new SelectList(namHocs, "MaNamHoc", "TenNamHoc", hocKy.MaNamHoc);
                return View(hocKy);
            }

            var existingHocKy = await _hocKyRepository.GetByIdAsync(id);
            if (existingHocKy == null)
            {
                return NotFound();
            }

            // Cập nhật dữ liệu
            existingHocKy.TenHocKy = hocKy.TenHocKy;
            existingHocKy.MaNamHoc = hocKy.MaNamHoc;

            await _hocKyRepository.UpdateAsync(existingHocKy);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị form xác nhận xóa học kỳ
        public async Task<IActionResult> Delete(int id)
        {
            var hocKy = await _hocKyRepository.GetByIdAsync(id);
            if (hocKy == null)
            {
                return NotFound();
            }
            return View(hocKy);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hocKy = await _hocKyRepository.GetByIdAsync(id);
            if (hocKy != null)  // Không return NotFound()
            {
                await _hocKyRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
