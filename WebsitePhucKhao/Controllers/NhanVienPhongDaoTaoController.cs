using Microsoft.AspNetCore.Mvc;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.ViewModels;
using WebsitePhucKhao.Enums;

namespace WebsitePhucKhao.Controllers {
    public class NhanVienPhongDaoTaoController : Controller {
        private readonly INhanVienPhongDaoTaoRepository _nhanVienRepository;
        private readonly ApplicationDbContext _context;

        public NhanVienPhongDaoTaoController(INhanVienPhongDaoTaoRepository nhanVienRepository, ApplicationDbContext context)
        {
            _nhanVienRepository = nhanVienRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nhanViens = await _nhanVienRepository.GetAllAsync();
            return View(nhanViens);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NhanVienPhongDaoTao nhanVien)
        {
            if (ModelState.IsValid)
            {
                await _nhanVienRepository.AddAsync(nhanVien);
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }

        public async Task<IActionResult> Update(int id)
        {
            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, NhanVienPhongDaoTao nhanVien)
        {
            if (ModelState.IsValid)
            {
                await _nhanVienRepository.UpdateAsync(nhanVien);
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVien);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index"); 
            }

            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _nhanVienRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChiTietSauPhucKhao(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.ChiTietPhucKhao)
                .Include(d => d.HinhAnhBaiThis)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null) return NotFound();

            var chiTiet = don.ChiTietPhucKhao;

            var vm = new DonPhucKhaoSauKhiChamViewModel
            {
                MaDon = don.MaDon,
                NgayGui = don.NgayGui,
                TrangThai = don.TrangThai.ToFriendlyString(),
                DiemHienTai = don.DiemHienTai,
                DiemSauPhucKhao = chiTiet?.DiemSauPhucKhao,
                NhanXet = chiTiet?.NhanXet,
                BaiGiaiTayUrl = chiTiet?.BaiGiaiTayUrl,
                DanhSachAnh = don.HinhAnhBaiThis?.ToList() ?? new List<HinhAnhBaiThi>()
            };

            return View(vm);
        }

    }
}
