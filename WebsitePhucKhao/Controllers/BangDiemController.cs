using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers {
    public class BangDiemController : Controller {
        private readonly IBangDiemRepository _bangDiemRepository;
        private readonly ISinhVienRepository _sinhVienRepository;
        private readonly IMonHocRepository _monHocRepository;

        public BangDiemController(IBangDiemRepository bangDiemRepository, ISinhVienRepository sinhVienRepository, IMonHocRepository monHocRepository)
        {
            _bangDiemRepository = bangDiemRepository;
            _sinhVienRepository = sinhVienRepository;
            _monHocRepository = monHocRepository;
        }

        public async Task<IActionResult> Index()
        {
            var bangDiems = await _bangDiemRepository.GetAllAsync();
            return View(bangDiems);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.SinhVienList = new SelectList(await _sinhVienRepository.GetAllAsync(), "MaSinhVien", "HoTen");
            ViewBag.MonHocList = new SelectList(await _monHocRepository.GetAllAsync(), "MaMonHoc", "TenMonHoc");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(BangDiem bangDiem)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SinhVienList = new SelectList(await _sinhVienRepository.GetAllAsync(), "MaSinhVien", "HoTen");
                ViewBag.MonHocList = new SelectList(await _monHocRepository.GetAllAsync(), "MaMonHoc", "TenMonHoc");
                return View(bangDiem);
            }

            await _bangDiemRepository.AddAsync(bangDiem);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var bangDiem = await _bangDiemRepository.GetByIdAsync(id);
            if (bangDiem == null)
            {
                return NotFound();
            }

            ViewBag.SinhVienList = new SelectList(await _sinhVienRepository.GetAllAsync(), "MaSinhVien", "HoTen", bangDiem.MaSinhVien);
            ViewBag.MonHocList = new SelectList(await _monHocRepository.GetAllAsync(), "MaMonHoc", "TenMonHoc", bangDiem.MaMonHoc);

            return View(bangDiem);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, BangDiem bangDiem)
        {
            if (!ModelState.IsValid)
            {
                return View(bangDiem);
            }

            await _bangDiemRepository.UpdateAsync(bangDiem);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var bangDiem = await _bangDiemRepository.GetByIdAsync(id);
            if (bangDiem == null)
            {
                return NotFound();
            }
            return View(bangDiem);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bangDiemRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
