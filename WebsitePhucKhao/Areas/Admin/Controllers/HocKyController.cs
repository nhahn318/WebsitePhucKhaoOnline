using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Threading.Tasks;

namespace WebsitePhucKhao.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            ViewBag.NamHocList = new SelectList(await _namHocRepository.GetAllAsync(), "MaNamHoc", "TenNamHoc");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(HocKy hocKy)
        {
            if (ModelState.IsValid)
            {
                await _hocKyRepository.AddAsync(hocKy);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.NamHocList = new SelectList(await _namHocRepository.GetAllAsync(), "MaNamHoc", "TenNamHoc");
            return View(hocKy);
        }

        public async Task<IActionResult> Update(int id)
        {
            var hocKy = await _hocKyRepository.GetByIdAsync(id);
            if (hocKy == null)
            {
                return NotFound();
            }
            ViewBag.NamHocList = new SelectList(await _namHocRepository.GetAllAsync(), "MaNamHoc", "TenNamHoc");
            return View(hocKy);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HocKy hocKy)
        {
            if (ModelState.IsValid)
            {
                await _hocKyRepository.UpdateAsync(hocKy);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.NamHocList = new SelectList(await _namHocRepository.GetAllAsync(), "MaNamHoc", "TenNamHoc");
            return View(hocKy);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _hocKyRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 