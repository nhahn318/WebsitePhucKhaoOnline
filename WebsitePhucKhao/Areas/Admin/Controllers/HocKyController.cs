using Microsoft.AspNetCore.Mvc;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace WebsitePhucKhao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HocKyController : Controller
    {
        private readonly IHocKyRepository _hocKyRepository;
        private readonly INamHocRepository _namHocRepository;

        public HocKyController(IHocKyRepository hocKyRepository, INamHocRepository namHocRepository)
        {
            _hocKyRepository = hocKyRepository;
            _namHocRepository = namHocRepository;
        }

        public async Task<IActionResult> Index()
        {
            var hocKys = await _hocKyRepository.GetAllAsync();
            return View(hocKys);
        }

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
            existingHocKy.NgayBatDauPhucKhao = hocKy.NgayBatDauPhucKhao;
            existingHocKy.NgayKetThucPhucKhao = hocKy.NgayKetThucPhucKhao;

            await _hocKyRepository.UpdateAsync(existingHocKy);
            return RedirectToAction(nameof(Index));
        }

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
            if (hocKy != null)  
            {
                await _hocKyRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
