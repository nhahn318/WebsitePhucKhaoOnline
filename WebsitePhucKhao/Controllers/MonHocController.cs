using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers {
    [Authorize(Roles = "Admin")]
    public class MonHocController : Controller
    {
        private readonly IMonHocRepository _monHocRepository;
        private readonly IHocKyRepository _hocKyRepository;
        private readonly IGiangVienRepository _giangVienRepository;

        public MonHocController(IMonHocRepository monHocRepository, IHocKyRepository hocKyRepository, IGiangVienRepository giangVienRepository)
        {
            _monHocRepository = monHocRepository;
            _hocKyRepository = hocKyRepository;
            _giangVienRepository = giangVienRepository;
        }

        public async Task<IActionResult> Index()
        {
            var monHocs = await _monHocRepository.GetAllAsync();
            return View(monHocs);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.HocKyList = new SelectList(await _hocKyRepository.GetAllAsync(), "MaHocKy", "TenHocKy");
            ViewBag.GiangVienList = new SelectList(await _giangVienRepository.GetAllAsync(), "MaGiangVien", "HoTen");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MonHoc monHoc)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HocKyList = new SelectList(await _hocKyRepository.GetAllAsync(), "MaHocKy", "TenHocKy");
                ViewBag.GiangVienList = new SelectList(await _giangVienRepository.GetAllAsync(), "MaGiangVien", "HoTen");
                return View(monHoc);
            }

            await _monHocRepository.AddAsync(monHoc);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var monHoc = await _monHocRepository.GetByIdAsync(id);
            if (monHoc == null)
            {
                return NotFound();
            }
            return View(monHoc);
        }

        public async Task<IActionResult> Update(int id)
        {
            var monHoc = await _monHocRepository.GetByIdAsync(id);
            if (monHoc == null)
            {
                return NotFound();
            }

            ViewBag.HocKyList = new SelectList(await _hocKyRepository.GetAllAsync(), "MaHocKy", "TenHocKy", monHoc.MaHocKy);
            ViewBag.GiangVienList = new SelectList(await _giangVienRepository.GetAllAsync(), "MaGiangVien", "HoTen", monHoc.MaGiangVien);

            return View(monHoc);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, MonHoc monHoc)
        {
            if (!ModelState.IsValid)
            {
                return View(monHoc);
            }

            await _monHocRepository.UpdateAsync(monHoc);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var monHoc = await _monHocRepository.GetByIdAsync(id);
            if (monHoc == null)
            {
                return NotFound();
            }
            return View(monHoc);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _monHocRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetMonHocByHocKy(int maHocKy)
        {
            var monHocs = await _monHocRepository.GetByHocKyAsync(maHocKy);
            return Json(monHocs);
        }
    }
}
