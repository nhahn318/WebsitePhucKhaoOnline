using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers {
    public class NamHocController : Controller {
        private readonly INamHocRepository _namHocRepository;

        public NamHocController(INamHocRepository namHocRepository)
        {
            _namHocRepository = namHocRepository;
        }

        // Hiển thị danh sách năm học
        public async Task<IActionResult> Index()
        {
            var namHocs = await _namHocRepository.GetAllAsync();
            return View(namHocs);
        }

        // Hiển thị form thêm năm học mới
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NamHoc namHoc)
        {
            if (!ModelState.IsValid)
            {
                return View(namHoc);
            }

            // Kiểm tra xem năm học đã tồn tại chưa
            var existingNamHoc = await _namHocRepository.GetByIdAsync(namHoc.MaNamHoc);
            if (existingNamHoc != null)
            {
                ModelState.AddModelError("MaNamHoc", "Mã năm học đã tồn tại!");
                return View(namHoc);
            }

            await _namHocRepository.AddAsync(namHoc);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị thông tin chi tiết năm học
        public async Task<IActionResult> Details(int id)
        {
            var namHoc = await _namHocRepository.GetByIdAsync(id);
            if (namHoc == null)
            {
                return NotFound();
            }
            return View(namHoc);
        }

        public async Task<IActionResult> Update(int id)
        {
            var namHoc = await _namHocRepository.GetByIdAsync(id);
            if (namHoc == null)
            {
                return NotFound();
            }

            return View(namHoc);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, NamHoc namHoc)
        {
            if (ModelState.IsValid)
            {
                var existingNamHoc = await _namHocRepository.GetByIdAsync(id);
                if (existingNamHoc == null)
                {
                    return NotFound();
                }

                existingNamHoc.TenNamHoc = namHoc.TenNamHoc; // Giả sử NamHoc có thuộc tính này

                await _namHocRepository.UpdateAsync(existingNamHoc);
                return RedirectToAction(nameof(Index));
            }

            return View(namHoc);
        }

        // Hiển thị form xác nhận xóa năm học
        public async Task<IActionResult> Delete(int id)
        {
            var namHoc = await _namHocRepository.GetByIdAsync(id);
            if (namHoc == null)
            {
                return NotFound();
            }
            return View(namHoc);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var namHoc = await _namHocRepository.GetByIdAsync(id);
            if (namHoc != null)
            {
                await _namHocRepository.DeleteAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
