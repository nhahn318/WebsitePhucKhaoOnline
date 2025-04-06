using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers {
    [Authorize(Roles = "Admin")]
    public class ChuyenNganhController : Controller {
        private readonly IChuyenNganhRepository _chuyenNganhRepository;
        private readonly IKhoaRepository _khoaRepository;

        public ChuyenNganhController(IChuyenNganhRepository chuyenNganhRepository, IKhoaRepository khoarepository)
        {
            _chuyenNganhRepository = chuyenNganhRepository;
            _khoaRepository = khoarepository;
        }

        // Hiển thị danh sách chuyên ngành
        public async Task<IActionResult> Index()
        {
            var chuyenNganhs = await _chuyenNganhRepository.GetAllAsync();
            return View(chuyenNganhs);
        }

        public async Task<IActionResult> Add()
        {
            var khoaList = await _khoaRepository.GetAllAsync(); // Lấy danh sách khoa từ DB
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa"); // Tạo dropdown list
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ChuyenNganh chuyenNganh)
        {
            if (!ModelState.IsValid)
            {
                // ✅ Ghi log lỗi nếu ModelState không hợp lệ
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Validation Errors: " + string.Join(", ", errors));
                return View(chuyenNganh);
            }

            await _chuyenNganhRepository.AddAsync(chuyenNganh);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị thông tin chi tiết chuyên ngành
        public async Task<IActionResult> Details(int id)
        {
            var chuyenNganh = await _chuyenNganhRepository.GetByIdAsync(id);
            if (chuyenNganh == null)
            {
                return NotFound();
            }
            return View(chuyenNganh);
        }

        // Hiển thị form cập nhật chuyên ngành
        public async Task<IActionResult> Update(int id)
        {
            var chuyenNganh = await _chuyenNganhRepository.GetByIdAsync(id);
            if (chuyenNganh == null)
            {
                return NotFound();
            }

            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa", chuyenNganh.MaKhoa); // Giữ Khoa hiện tại

            return View(chuyenNganh);
        }

        // Xử lý cập nhật chuyên ngành
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ChuyenNganh chuyenNganh)
        {
            if (ModelState.IsValid)
            {
                var existingChuyenNganh = await _chuyenNganhRepository.GetByIdAsync(id);
                if (existingChuyenNganh == null)
                {
                    return NotFound();
                }

                existingChuyenNganh.TenChuyenNganh = chuyenNganh.TenChuyenNganh;
                existingChuyenNganh.MaKhoa = chuyenNganh.MaKhoa;

                await _chuyenNganhRepository.UpdateAsync(existingChuyenNganh);
                return RedirectToAction(nameof(Index));
            }

            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa", chuyenNganh.MaKhoa);

            return View(chuyenNganh);
        }


        // Hiển thị form xác nhận xóa chuyên ngành
        public async Task<IActionResult> Delete(int id)
        {
            var chuyenNganh = await _chuyenNganhRepository.GetByIdAsync(id);
            if (chuyenNganh == null)
            {
                return NotFound();
            }
            return View(chuyenNganh);
        }

        // Xử lý xóa chuyên ngành
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _chuyenNganhRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
