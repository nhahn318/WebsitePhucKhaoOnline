using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers {
    public class PhucKhaoController : Controller {
        private readonly IPhucKhaoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhucKhaoController(IPhucKhaoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        // Sinh viên - danh sách
        public async Task<IActionResult> DanhSachDonPhucKhao()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var sv = await _repository.GetSinhVienByEmailAsync(email);
            if (sv == null) return NotFound("Không tìm thấy sinh viên.");

            var ds = await _repository.GetDanhSachPhucKhaoSinhVienAsync(sv.MaSinhVien);

            var viewData = ds.Select((d, i) => new DanhSachDonPhucKhaoViewModel
            {
                STT = i + 1,
                MaDon = d.MaDon,
                NgayGui = d.NgayGui,
                TrangThai = d.TrangThai
            }).ToList();

            return View(viewData);
        }

        public async Task<IActionResult> Details(int id)
        {
            var don = await _repository.GetDonPhucKhaoAsync(id);
            if (don == null) return NotFound();
            return View(don);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _repository.GetSuaPhucKhaoViewModelAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SuaPhucKhaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.CapNhatDonPhucKhaoAsync(model))
                    return RedirectToAction("DanhSachDonPhucKhao");
            }

            model.DanhSachMonHoc = new SelectList(await _repository.GetMonHocListAsync(), "MaMonHoc", "TenMonHoc");
            model.DanhSachHocKy = new SelectList(await _repository.GetHocKyListAsync(), "MaHocKy", "TenHocKy");
            model.DanhSachNamHoc = new SelectList(await _repository.GetNamHocListAsync(), "MaNamHoc", "TenNamHoc");
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!await _repository.XacNhanXoaDonAsync(id))
                return NotFound();

            var don = await _repository.GetDonPhucKhaoAsync(id);
            return View(don);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int maDon)
        {
            await _repository.XoaDonAsync(maDon);
            return RedirectToAction("DanhSachDonPhucKhao");
        }

        // Tạo đơn
        public async Task<IActionResult> Create()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.ErrorMessage = "Bạn chưa đăng nhập.";
                return View();
            }

            var vm = await _repository.GetYeuCauPhucKhaoViewModelAsync(email);
            if (vm == null)
            {
                ViewBag.ErrorMessage = "Hiện tại không phải là thời gian phúc khảo hoặc sinh viên không hợp lệ.";
                return View("OutsidePhucKhao");
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YeuCauPhucKhaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _repository.TaoDonPhucKhaoAsync(model);
                return RedirectToAction("CreateSuccess");
            }

            model.DanhSachMonHoc = new SelectList(await _repository.GetMonHocListAsync(), "MaMonHoc", "TenMonHoc");
            model.DanhSachHocKy = new SelectList(await _repository.GetHocKyListAsync(), "MaHocKy", "TenHocKy");
            return View(model);
        }

        public IActionResult CreateSuccess() => View();

        // Nhân viên: danh sách đơn
        public async Task<IActionResult> DanhSachChoDuyet()
        {
            var ds = await _repository.GetDanhSachChoDuyetAsync();
            return View(ds);
        }
        
        //=============================================================NhanVienPhongDaoTao===============================================

        public async Task<IActionResult> ChiTietPhucKhaoChoNhanVien(int id)
        {
            var don = await _repository.GetDonPhucKhaoAsync(id);
            if (don == null) return NotFound();

            return View("ChiTietPhucKhaoChoNhanVien", don);
        }

        public async Task<IActionResult> Duyet(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.MaNhanVienPhongDaoTao == null) return Unauthorized();

            await _repository.DuyetDonAsync(id, user.MaNhanVienPhongDaoTao);
            return RedirectToAction("DanhSachChoDuyet");
        }

        // Upload bài thi
        public async Task<IActionResult> UploadBaiThi(int maDon)
        {
            var vm = await _repository.GetUploadViewModelAsync(maDon);
            if (vm == null) return NotFound();

            ViewBag.GiangViens = new SelectList(await _repository.GetGiangVienListAsync(), "MaGiangVien", "HoTen");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadBaiThi(UploadBaiThiViewModel model, List<IFormFile> files)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                ViewBag.GiangViens = new SelectList(await _repository.GetGiangVienListAsync(), "MaGiangVien", "HoTen", model.MaGiangVien);
                return View(model);
            }

            await _repository.LuuUploadAsync(model, files, user?.MaNhanVienPhongDaoTao);
            return RedirectToAction("DanhSachChoDuyet");
        }

        public async Task<IActionResult> ChiTietUpload(int maDon)
        {
            var vm = await _repository.GetChiTietUploadAsync(maDon);
            if (vm == null) return NotFound();
            return View(vm);
        }

        public async Task<IActionResult> EditUpload(int maDon)
        {
            var vm = await _repository.GetEditUploadAsync(maDon);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUpload(EditUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DanhSachAnh = (await _repository.GetChiTietUploadAsync(model.MaDon))?.DanhSachAnh ?? new();
                model.DanhSachGiangVien = new SelectList(await _repository.GetGiangVienListAsync(), "MaGiangVien", "HoTen", model.MaGiangVien);
                return View(model);
            }

            await _repository.LuuChinhSuaUploadAsync(model);
            return RedirectToAction("ChiTietUpload", new { maDon = model.MaDon });
        }

        public async Task<IActionResult> DanhSachPhucKhaoSauKhiCham()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var sinhVien = await _repository.GetSinhVienByEmailAsync(email);
            if (sinhVien == null) return NotFound();

            var danhSach = await _repository.GetDanhSachPhucKhaoViewModelAsync(sinhVien.MaSinhVien);

            var daCham = danhSach.Where(d => d.TrangThai == "Đã chấm").ToList();

            return View(daCham);
        }
    }
}
