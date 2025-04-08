using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using WebsitePhucKhao.ViewModels;

namespace WebsitePhucKhao.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    [Authorize(Roles = "SinhVien")]
    public class PhucKhaoController : Controller {
        private readonly IPhucKhaoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhucKhaoController(IPhucKhaoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

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

            vm.DanhSachMonHoc = new SelectList(await _repository.GetMonHocListAsync(), "MaMonHoc", "TenMonHoc");
            vm.DanhSachHocKy = new SelectList(await _repository.GetHocKyListAsync(), "MaHocKy", "TenHocKy");
            vm.DanhSachNamHoc = new SelectList(await _repository.GetNamHocListAsync(), "MaNamHoc", "TenNamHoc");

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

        public async Task<IActionResult> Details(int id)
        {
            var don = await _repository.GetDonPhucKhaoAsync(id);
            if (don == null) return NotFound();
            return View(don);
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
