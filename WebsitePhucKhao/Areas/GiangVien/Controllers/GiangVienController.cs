using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Enums;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using WebsitePhucKhao.ViewModels;

namespace WebsitePhucKhao.Areas.GiangVien.Controllers {
    [Area("GiangVien")]
    [Authorize(Roles = "GiangVien")]
    public class GiangVienController : Controller {

        private readonly IGiangVienRepository _giangVienRepository;
        private readonly IKhoaRepository _khoaRepository;
        private readonly IDonPhucKhaoChiTietRepository _donPhucKhaoChiTietRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ICustomEmailSender _emailSender;

        public GiangVienController(IGiangVienRepository giangVienRepository, IKhoaRepository khoaRepository, IDonPhucKhaoChiTietRepository donPhucKhaoChiTietRepository, UserManager<ApplicationUser> userManager, ApplicationDbContext context, ICustomEmailSender emailSender)
        {
            _giangVienRepository = giangVienRepository;
            _khoaRepository = khoaRepository;
            _donPhucKhaoChiTietRepository = donPhucKhaoChiTietRepository;
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> ThongTinCaNhan()
        {
            var user = await _userManager.GetUserAsync(User);
            var gv = await _giangVienRepository.GetByEmailAsync(user.Email);

            if (gv == null) return NotFound();

            return View(gv);
        }

        [HttpGet]
        public async Task<IActionResult> EditThongTinCaNhan()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var gv = await _giangVienRepository.GetByEmailAsync(user.Email);
            if (gv == null) return NotFound();

            var model = new GiangVienEditViewModel
            {
                MaGiangVien = gv.MaGiangVien,
                HoTen = gv.HoTen,
                SoDienThoai = user.PhoneNumber
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditThongTinCaNhan(GiangVienEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var giangVien = await _giangVienRepository.GetByIdAsync(model.MaGiangVien);
            if (giangVien == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            giangVien.HoTen = model.HoTen;
            user.PhoneNumber = model.SoDienThoai;

            await _giangVienRepository.UpdateAsync(giangVien);
            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";
            return RedirectToAction("ThongTinCaNhan");
        }



        public async Task<IActionResult> PhucKhaoDuocPhanCong()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var giangVien = await _giangVienRepository.GetByEmailAsync(user.Email);
            if (giangVien == null) return RedirectToAction("AccessDenied", "Account");

            var danhSachDon = await _donPhucKhaoChiTietRepository.GetPhucKhaoByGiangVienAsync(giangVien.MaGiangVien);

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

        public async Task<IActionResult> PhucKhaoDaCham()
        {
            var user = await _userManager.GetUserAsync(User);
            var gv = await _giangVienRepository.GetByEmailAsync(user.Email);

            var ds = await _donPhucKhaoChiTietRepository.GetDaChamByGiangVienAsync(gv.MaGiangVien);

            var vm = ds.Select((d, i) => new PhucKhaoDuocPhanCongViewModel
            {
                MaDon = d.MaDon,
                DiemTruocPhucKhao = d.DonPhucKhao?.DiemHienTai ?? 0,
                DiemMongMuon = d.DonPhucKhao?.DiemMongMuon ?? 0,
                LyDo = d.DonPhucKhao?.LyDo ?? "Không có lý do",
                TenMonHoc = d.MonHoc?.TenMonHoc ?? "Không rõ",
                DiemSauPhucKhao = d.DiemSauPhucKhao,
                TrangThaiPhucKhao = d.DonPhucKhao?.TrangThai ?? TrangThaiPhucKhao.DaCham,
                NgayChamLai = d.NgayChamLai,
                NguoiDuyet = d.NhanVienDuyet?.HoTen ?? "Chưa duyệt"
            }).ToList();

            return View(vm);
        }

        //khi giảng viên chấm điểm và bấm nút "Lưu điểm" sẽ lưu lại và gửi mail cho sinh viên
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
                DiemSauPhucKhao = chiTiet?.DiemSauPhucKhao,
                BaiGiaiTayUrl = chiTiet?.BaiGiaiTayUrl
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
                _context.Update(chiTiet);
            }

            var don = await _context.DonPhucKhaos.FirstOrDefaultAsync(d => d.MaDon == MaDon);
            if (don != null && don.EmailSinhVien != null)
            {
                string subject = "Thông báo kết quả phúc khảo";
                string body = $@"
                    Xin chào {don.SinhVien?.HoTen},

                    Đơn phúc khảo của bạn đối với môn học {don.MonHoc?.TenMonHoc} đã được chấm lại.

                    👉 Điểm sau phúc khảo: {DiemSauPhucKhao}

                    Vui lòng đăng nhập hệ thống để xem chi tiết.

                    Trân trọng,
                    Phòng đào tạo
                ";

                await _emailSender.SendEmailAsync(don.EmailSinhVien, subject, body);
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
