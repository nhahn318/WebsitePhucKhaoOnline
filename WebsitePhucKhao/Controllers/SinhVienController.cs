using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers
{
    public class SinhVienController : Controller {
        private readonly ISinhVienRepository _sinhVienRepository;
        private readonly IKhoaRepository _khoaRepository;
        private readonly IChuyenNganhRepository _chuyenNganhRepository;

        public SinhVienController(ISinhVienRepository sinhVienRepository, IKhoaRepository khoaRepository, IChuyenNganhRepository chuyenNganhRepository)
        {
            _sinhVienRepository = sinhVienRepository;
            _khoaRepository = khoaRepository;
            _chuyenNganhRepository = chuyenNganhRepository;
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

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SinhVien sinhVien)
        {
            if (!ModelState.IsValid)
            {
                var khoaList = await _khoaRepository.GetAllAsync();
                ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");

                ViewBag.ChuyenNganhList = new List<SelectListItem>();

                return View(sinhVien);
            }

            //Kiểm tra xem mã sinh viên đã tồn tại chưa
            var existingSinhVien = await _sinhVienRepository.GetByIdAsync(sinhVien.MaSinhVien);
            if (existingSinhVien != null)
            {
                ModelState.AddModelError("MaSinhVien", "Mã sinh viên đã tồn tại!");
                return View(sinhVien);
            }

            //Đặt mật khẩu mặc định là mã sinh viên
            sinhVien.MatKhau = sinhVien.MaSinhVien.ToString();

            await _sinhVienRepository.AddAsync(sinhVien);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị thông tin chi tiết sinh viên
        public async Task<IActionResult> Details(int id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        public async Task<IActionResult> Update(int id)
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
        public async Task<IActionResult> Update(int id, SinhVien sinhVien)
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
        public async Task<IActionResult> Delete(int id)
        {
            var sinhVien = await _sinhVienRepository.GetByIdAsync(id);
            if (sinhVien == null)
            {
                return NotFound();
            }
            return View(sinhVien);
        }

        // Xử lý xóa sinh viên
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sinhVienRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<JsonResult> GetChuyenNganhByKhoa(int maKhoa)
        {
            var chuyenNganhs = await _chuyenNganhRepository.GetByKhoaIdAsync(maKhoa);
            return Json(chuyenNganhs);
        }


    }
}
