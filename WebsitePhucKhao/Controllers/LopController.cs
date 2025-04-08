using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers {
    [Authorize(Roles = "Admin")]
    public class LopController : Controller {
        private readonly ILopRepository _lopRepository;
        private readonly IKhoaRepository _khoaRepository;

        public LopController(ILopRepository lopRepository, IKhoaRepository khoaRepository)
        {
            _lopRepository = lopRepository;
            _khoaRepository = khoaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var lops = await _lopRepository.GetAllAsync();
            return View(lops);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.KhoaList = new SelectList(await _khoaRepository.GetAllAsync(), "MaKhoa", "TenKhoa");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Lop lop)
        {
            if (ModelState.IsValid)
            {
                await _lopRepository.AddAsync(lop);
                return RedirectToAction("Index");
            }

            ViewBag.KhoaList = new SelectList(await _khoaRepository.GetAllAsync(), "MaKhoa", "TenKhoa");
            return View(lop);
        }

        public async Task<IActionResult> Update(int id)
        {
            var lop = await _lopRepository.GetByIdAsync(id);
            if (lop == null) return NotFound();

            ViewBag.KhoaList = new SelectList(await _khoaRepository.GetAllAsync(), "MaKhoa", "TenKhoa", lop.MaKhoa);
            return View(lop);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Lop lop)
        {
            if (ModelState.IsValid)
            {
                await _lopRepository.UpdateAsync(lop);
                return RedirectToAction("Index");
            }

            ViewBag.KhoaList = new SelectList(await _khoaRepository.GetAllAsync(), "MaKhoa", "TenKhoa", lop.MaKhoa);
            return View(lop);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var lop = await _lopRepository.GetByIdAsync(id);
            if (lop == null) return NotFound();
            return View(lop);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _lopRepository.DeleteAsync(id);
                TempData["Message"] = "Xóa lớp thành công!";
            }
            catch (DbUpdateException ex)
            {
                TempData["Error"] = "Không thể xóa lớp vì có sinh viên thuộc lớp này.";
                Console.WriteLine("Lỗi khi xóa lớp: " + ex.Message); // Ghi log lỗi ra console
            }
            return RedirectToAction("Index");
        }


    }
}
