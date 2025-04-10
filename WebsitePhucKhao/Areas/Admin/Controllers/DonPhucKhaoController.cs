using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using WebsitePhucKhao.Enums;
using NuGet.Protocol.Core.Types;

namespace WebsitePhucKhao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonPhucKhaoController : Controller
    {
        private readonly IPhucKhaoRepository _phucKhaoRepository;
        private readonly IPhucKhaoRepository _repository;

        public DonPhucKhaoController(IPhucKhaoRepository phucKhaoRepository, IPhucKhaoRepository repository)
        {
            _phucKhaoRepository = phucKhaoRepository;
            _repository = repository;
        }

        public async Task<IActionResult> Index(string searchTerm, int? trangThai, int? hocKy, int? monHoc)
        {
            var donPhucKhaos = await _phucKhaoRepository.GetDanhSachDonAsync();

            // Lọc theo trạng thái
            if (trangThai.HasValue)
            {
                donPhucKhaos = donPhucKhaos.Where(d => (int)d.TrangThai == trangThai.Value).ToList();
            }

            // Lọc theo học kỳ
            if (hocKy.HasValue)
            {
                donPhucKhaos = donPhucKhaos.Where(d => d.MaHocKy == hocKy.Value).ToList();
            }

            // Lọc theo môn học
            if (monHoc.HasValue)
            {
                donPhucKhaos = donPhucKhaos.Where(d => d.MaMonHoc == monHoc.Value).ToList();
            }

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                donPhucKhaos = donPhucKhaos.Where(d => 
                    d.SinhVien?.HoTen.Contains(searchTerm) == true ||
                    d.SinhVien?.MaSinhVien.ToString().Contains(searchTerm) == true ||
                    d.MonHoc?.TenMonHoc.Contains(searchTerm) == true
                ).ToList();
            }

            // Lấy danh sách cho dropdown
            ViewBag.HocKys = await _phucKhaoRepository.GetHocKyListAsync();
            ViewBag.MonHocs = await _phucKhaoRepository.GetMonHocListAsync();
            ViewBag.NamHocs = await _phucKhaoRepository.GetNamHocListAsync();
            ViewBag.GiangViens = await _phucKhaoRepository.GetGiangVienListAsync();

            // Lưu các giá trị lọc
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedTrangThai = trangThai;
            ViewBag.SelectedHocKy = hocKy;
            ViewBag.SelectedMonHoc = monHoc;

            return View(donPhucKhaos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var don = await _phucKhaoRepository.GetDonPhucKhaoAsync(id);
            if (don == null)
            {
                return NotFound();
            }
            return View(don);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.HocKys = await _phucKhaoRepository.GetHocKyListAsync();
            ViewBag.MonHocs = await _phucKhaoRepository.GetMonHocListAsync();
            ViewBag.NamHocs = await _phucKhaoRepository.GetNamHocListAsync();
            ViewBag.GiangViens = await _phucKhaoRepository.GetGiangVienListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonPhucKhao don)
        {
            if (ModelState.IsValid)
            {
                don.TrangThai = TrangThaiPhucKhao.ChoXacNhan;
                don.NgayGui = DateTime.Now;
                _phucKhaoRepository.Update(don);
                await _phucKhaoRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.HocKys = await _phucKhaoRepository.GetHocKyListAsync();
            ViewBag.MonHocs = await _phucKhaoRepository.GetMonHocListAsync();
            ViewBag.NamHocs = await _phucKhaoRepository.GetNamHocListAsync();
            ViewBag.GiangViens = await _phucKhaoRepository.GetGiangVienListAsync();

            return View(don);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var don = await _phucKhaoRepository.GetDonPhucKhaoAsync(id);
            if (don == null)
            {
                return NotFound();
            }
            return Json(don);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, DonPhucKhao don)
        {
            if (id != don.MaDon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _phucKhaoRepository.Update(don);
                await _phucKhaoRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.HocKys = await _phucKhaoRepository.GetHocKyListAsync();
            ViewBag.MonHocs = await _phucKhaoRepository.GetMonHocListAsync();
            ViewBag.NamHocs = await _phucKhaoRepository.GetNamHocListAsync();
            ViewBag.GiangViens = await _phucKhaoRepository.GetGiangVienListAsync();

            return View(don);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var don = await _phucKhaoRepository.GetDonPhucKhaoAsync(id);
            if (don == null)
            {
                return NotFound();
            }

            await _phucKhaoRepository.XoaDonAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetNewNotifications()
        {
            var donPhucKhaos = await _phucKhaoRepository.GetDanhSachDonAsync();
            // Lọc chỉ những đơn chưa duyệt
            var donChuaDuyet = donPhucKhaos.Where(d => d.TrangThai == TrangThaiPhucKhao.ChoXacNhan).ToList();
            return Json(new { 
                count = donChuaDuyet.Count,
                notifications = donChuaDuyet.Select(n => new {
                    message = $"Có đơn phúc khảo mới từ sinh viên {n.SinhVien?.HoTen}",
                    time = n.NgayGui.ToString("dd/MM/yyyy HH:mm"),
                    maDon = n.MaDon,
                    maMonHoc = n.MaMonHoc
                })
            });
        }



        public async Task<IActionResult> DanhSachChoDuyet()
        {
            var ds = await _repository.GetDanhSachDonAsync();
            return View(ds);
        }

        public async Task<IActionResult> ChiTietPhucKhao(int id, int maMonHoc)
        {
            var don = await _repository.GetDonPhucKhaoAsync(id);
            if (don == null) return NotFound();

            return View("ChiTietPhucKhao", don);
        }

 
    }
} 