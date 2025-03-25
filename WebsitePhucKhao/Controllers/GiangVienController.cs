using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.ViewModels;
using Humanizer;
using WebsitePhucKhao.Enums;

namespace WebsitePhucKhao.Controllers {
    public class GiangVienController : Controller {
        private readonly IGiangVienRepository _giangVienRepository;
        private readonly IKhoaRepository _khoaRepository;
        private readonly IDonPhucKhaoChiTietRepository _DonPhucKhaoChiTietRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;



        public GiangVienController(IGiangVienRepository giangVienRepository, IKhoaRepository khoaRepository, IDonPhucKhaoChiTietRepository donPhucKhaoChiTietRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _giangVienRepository = giangVienRepository;
            _khoaRepository = khoaRepository;
            _DonPhucKhaoChiTietRepository = donPhucKhaoChiTietRepository;
            _userManager = userManager;
            _context = context;
        }

        // Hiển thị danh sách giảng viên
        public async Task<IActionResult> Index()
        {
            var giangViens = await _giangVienRepository.GetAllWithDetailsAsync();
            return View(giangViens);
        }

        // Hiển thị form thêm giảng viên mới
        public async Task<IActionResult> Add()
        {
            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GiangVien giangVien)
        {
            if (!ModelState.IsValid)
            {
                var khoaList = await _khoaRepository.GetAllAsync();
                ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa");
                return View(giangVien);
            }

            await _giangVienRepository.AddAsync(giangVien);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị thông tin chi tiết giảng viên
        public async Task<IActionResult> Details(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }
            return View(giangVien);
        }

        public async Task<IActionResult> Update(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }

            var khoaList = await _khoaRepository.GetAllAsync();
            ViewBag.KhoaList = new SelectList(khoaList, "MaKhoa", "TenKhoa", giangVien.MaKhoa);

            return View(giangVien);
        }

        [HttpPost]
        public async Task<IActionResult> Update(long id, GiangVien giangVien)
        {
            if (ModelState.IsValid)
            {
                var existingGiangVien = await _giangVienRepository.GetByIdAsync(id);
                if (existingGiangVien == null)
                {
                    return NotFound();
                }

                existingGiangVien.HoTen = giangVien.HoTen;
                existingGiangVien.Email = giangVien.Email;
                existingGiangVien.SoDienThoai = giangVien.SoDienThoai;
                existingGiangVien.MaKhoa = giangVien.MaKhoa;

                await _giangVienRepository.UpdateAsync(existingGiangVien);
                return RedirectToAction(nameof(Index));
            }

            return View(giangVien);
        }

        // Hiển thị form xác nhận xóa giảng viên
        public async Task<IActionResult> Delete(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien == null)
            {
                return NotFound();
            }
            return View(giangVien);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var giangVien = await _giangVienRepository.GetByIdAsync(id);
            if (giangVien != null)
            {
                await _giangVienRepository.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> PhucKhaoDuocPhanCong()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var giangVien = await _giangVienRepository.GetByEmailAsync(user.Email);
            if (giangVien == null) return RedirectToAction("AccessDenied", "Account");

            var danhSachDon = await _DonPhucKhaoChiTietRepository.GetPhucKhaoByGiangVienAsync(giangVien.MaGiangVien);

            // Chuyển đổi danh sách DonPhucKhaoChiTiet thành ViewModel
            var danhSachViewModel = danhSachDon.Select(d => new PhucKhaoDuocPhanCongViewModel
            {
                MaDon = d.MaDon,
                DiemTruocPhucKhao = d.DonPhucKhao?.DiemHienTai ?? 0,
                DiemMongMuon = d.DonPhucKhao?.DiemMongMuon ?? 0,           
                LyDo = d.DonPhucKhao?.LyDo ?? "Không có lý do",
                TenMonHoc = d.MonHoc?.TenMonHoc ?? "Không rõ",
                NgayChamLai = d.NgayChamLai,
                NguoiDuyet = d.NhanVienDuyet?.HoTen ?? "Chưa duyệt",
                DiemSauPhucKhao = d.DiemSauPhucKhao,
                TrangThaiPhucKhao = d.DonPhucKhao?.TrangThai ?? TrangThaiPhucKhao.ChoXacNhan
            }).ToList();

            return View(danhSachViewModel);
        }
        //====================================================================CHAM DIEM BAI PHUC KHAO===========================================

        public async Task<IActionResult> ChamDiem(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null) return NotFound();

            var chiTiet = await _context.DonPhucKhaoChiTiets
                .FirstOrDefaultAsync(ct => ct.MaDon == maDon);

            var hinhAnh = await _context.HinhAnhBaiThis
                .Where(h => h.MaDon == maDon)
                .ToListAsync();

            var viewModel = new ChamDiemViewModel
            {
                MaDon = maDon,
                TenSinhVien = don.SinhVien?.HoTen,
                TenMonHoc = don.MonHoc?.TenMonHoc,
                DiemTruocPhucKhao = don.DiemHienTai,
                DiemMongMuon = don.DiemMongMuon,
                LyDo = don.LyDo,
                DanhSachAnh = hinhAnh,
                DiemSauPhucKhao = chiTiet?.DiemSauPhucKhao
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChamDiem(int MaDon, float DiemSauPhucKhao, string? NhanXet, IFormFile? BaiGiaiTay)
        {
            var chiTiet = await _context.DonPhucKhaoChiTiets.FirstOrDefaultAsync(c => c.MaDon == MaDon);
            if (chiTiet == null) return NotFound();

            chiTiet.DiemSauPhucKhao = DiemSauPhucKhao;
            chiTiet.NgayChamLai = DateTime.Now;
            chiTiet.NhanXet = NhanXet;

            if (BaiGiaiTay != null && BaiGiaiTay.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bai-giai-tay");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = $"{MaDon}_{Path.GetFileName(BaiGiaiTay.FileName)}";
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await BaiGiaiTay.CopyToAsync(stream);
                }

                chiTiet.BaiGiaiTayUrl = "/bai-giai-tay/" + fileName; 
            }

            var don = await _context.DonPhucKhaos.FirstOrDefaultAsync(d => d.MaDon == MaDon);
            if (don != null)
            {
                don.TrangThai = TrangThaiPhucKhao.DaCham;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("PhucKhaoDuocPhanCong");
        }



        public async Task<IActionResult> DetailChamDiem(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            var chiTiet = await _context.DonPhucKhaoChiTiets
                .Include(ct => ct.GiangVien)
                .FirstOrDefaultAsync(ct => ct.MaDon == maDon);

            var hinhAnh = await _context.HinhAnhBaiThis
                .Where(h => h.MaDon == maDon)
                .ToListAsync();

            if (don == null || chiTiet == null)
                return NotFound();

            var viewModel = new ChamDiemViewModel
            {
                MaDon = maDon,
                TenSinhVien = don.SinhVien?.HoTen,
                TenMonHoc = don.MonHoc?.TenMonHoc,
                DiemTruocPhucKhao = don.DiemHienTai,
                DiemMongMuon = don.DiemMongMuon,
                DiemSauPhucKhao = chiTiet.DiemSauPhucKhao,
                NhanXet = chiTiet.NhanXet,
                DanhSachAnh = hinhAnh,
                BaiGiaiTayUrl = chiTiet.BaiGiaiTayUrl
            };

            return View(viewModel);
        }

        public async Task<IActionResult> EditChamDiem(int maDon)
        {
            var chiTiet = await _context.DonPhucKhaoChiTiets
                .Include(c => c.DonPhucKhao)
                .ThenInclude(d => d.SinhVien)
                .Include(c => c.DonPhucKhao.MonHoc)
                .FirstOrDefaultAsync(c => c.MaDon == maDon);

            if (chiTiet == null) return NotFound();

            var hinhAnh = await _context.HinhAnhBaiThis
                .Where(h => h.MaDon == maDon)
                .ToListAsync();

            var model = new ChamDiemViewModel
            {
                MaDon = maDon,
                TenSinhVien = chiTiet.DonPhucKhao?.SinhVien?.HoTen,
                TenMonHoc = chiTiet.DonPhucKhao?.MonHoc?.TenMonHoc,
                DiemTruocPhucKhao = chiTiet.DonPhucKhao?.DiemHienTai,
                DiemMongMuon = chiTiet.DonPhucKhao?.DiemMongMuon,
                DiemSauPhucKhao = chiTiet.DiemSauPhucKhao,
                NhanXet = chiTiet.NhanXet,
                BaiGiaiTayUrl = chiTiet.BaiGiaiTayUrl,
                DanhSachAnh = hinhAnh
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChamDiem(ChamDiemViewModel model)
        {
            var chiTiet = await _context.DonPhucKhaoChiTiets.FirstOrDefaultAsync(c => c.MaDon == model.MaDon);
            if (chiTiet == null) return NotFound();

            chiTiet.DiemSauPhucKhao = model.DiemSauPhucKhao;
            chiTiet.NhanXet = model.NhanXet;
            chiTiet.NgayChamLai = DateTime.Now;

            // Nếu có bài giải tay mới
            if (model.BaiGiaiTay != null && model.BaiGiaiTay.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bai-giai-tay");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                var fileName = $"{model.MaDon}_{Path.GetFileName(model.BaiGiaiTay.FileName)}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await model.BaiGiaiTay.CopyToAsync(stream);

                chiTiet.BaiGiaiTayUrl = "/bai-giai-tay/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("DetailChamDiem", new { maDon = model.MaDon });
        }




    }
}
