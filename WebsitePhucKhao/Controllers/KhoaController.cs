using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers {
    [Authorize(Roles = "Admin")]
    public class KhoaController : Controller {
        private readonly IKhoaRepository _khoaRepository;

        public KhoaController(IKhoaRepository khoaRepository)
        {
            _khoaRepository = khoaRepository;
        }

        // Hiển thị danh sách Khoa
        public async Task<IActionResult> Index()
        {
            var khoas = await _khoaRepository.GetAllAsync();
            return View(khoas);
        }

        // Hiển thị form thêm khoa mới
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Khoa khoa)
        {
            if (!ModelState.IsValid)
            {
                // ✅ Ghi log lỗi nếu ModelState không hợp lệ
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
                return View(khoa);
            }

            await _khoaRepository.AddAsync(khoa);
            return RedirectToAction(nameof(Index));
        }


        // Hiển thị thông tin chi tiết khoa
        public async Task<IActionResult> Details(int id)
        {
            var khoa = await _khoaRepository.GetByIdAsync(id);
            if (khoa == null)
            {
                return NotFound();
            }
            return View(khoa);
        }

        // Hiển thị form cập nhật khoa
        public async Task<IActionResult> Update(int id)
        {
            var khoa = await _khoaRepository.GetByIdAsync(id);
            if (khoa == null)
            {
                return NotFound();
            }
            return View(khoa);
        }

        // Xử lý cập nhật khoa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Khoa khoa)
        {
            if (ModelState.IsValid)
            {
                var existingKhoa = await _khoaRepository.GetByIdAsync(id);
                if (existingKhoa == null)
                {
                    return NotFound();
                }

                existingKhoa.TenKhoa = khoa.TenKhoa;

                await _khoaRepository.UpdateAsync(existingKhoa);
                return RedirectToAction(nameof(Index));
            }

            return View(khoa);
        }

        // Hiển thị form xác nhận xóa khoa
        public async Task<IActionResult> Delete(int id)
        {
            var khoa = await _khoaRepository.GetByIdAsync(id);
            if (khoa == null)
            {
                return NotFound();
            }
            return View(khoa);
        }

        // Xử lý xóa khoa
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _khoaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
