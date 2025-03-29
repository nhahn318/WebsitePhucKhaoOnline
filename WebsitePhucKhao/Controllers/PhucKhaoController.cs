using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebsitePhucKhao.Repositories;
using Microsoft.EntityFrameworkCore;


namespace WebsitePhucKhao.Controllers {
    public class PhucKhaoController : Controller {
        private readonly IPhucKhaoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomEmailSender _emailSen;
        private readonly ApplicationDbContext _context;

        public PhucKhaoController(IPhucKhaoRepository repository, UserManager<ApplicationUser> userManager, ICustomEmailSender emailSender, ApplicationDbContext context)
        {
            _repository = repository;
            _userManager = userManager;
            _emailSen = emailSender;
            _context = context;
        }

        // Sinh viên - danh sách
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

        public async Task<IActionResult> Details(int id)
        {
            var don = await _repository.GetDonPhucKhaoAsync(id);
            if (don == null) return NotFound();
            return View(don);
        }

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

        // Tạo đơn
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

        // Nhân viên: danh sách đơn
        public async Task<IActionResult> DanhSachChoDuyet()
        {
            var ds = await _repository.GetDanhSachChoDuyetAsync();
            return View(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetLichThiInfo(int maMonHoc, int maHocKy, int maNamHoc)
        {
            var lichThi = await _context.LichThis
                .Where(l => l.MaMonHoc == maMonHoc
                         && l.MaHocKy == maHocKy
                         && l.MaNamHoc == maNamHoc)
                .FirstOrDefaultAsync();

            if (lichThi == null)
            {
                return Json(null);
            }

            return Json(new
            {
                ngayThi = lichThi.NgayThi.ToString("yyyy-MM-dd"),
                caThi = lichThi.CaThi,
                phongThi = lichThi.PhongThi,
                diaDiemThi = lichThi.DiaDiemThi
            });
        }



        //=============================================================NhanVienPhongDaoTao===============================================

        public async Task<IActionResult> ChiTietPhucKhaoChoNhanVien(int id)
        {
            var don = await _repository.GetDonPhucKhaoAsync(id);
            if (don == null) return NotFound();

            return View("ChiTietPhucKhaoChoNhanVien", don);
        }

        public async Task<IActionResult> Duyet(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.MaNhanVienPhongDaoTao == null) return Unauthorized();

            await _repository.DuyetDonAsync(id, user.MaNhanVienPhongDaoTao);
            // Lấy lại đơn để lấy email sinh viên
            var don = await _repository.GetDonPhucKhaoAsync(id);
            if (don != null && !string.IsNullOrEmpty(don.EmailSinhVien) && !don.DaGuiEmail)
            {
                var subject = "Thông báo duyệt đơn phúc khảo";
                var message = $"""
                Xin chào {don.SinhVien?.HoTen},

                Đơn phúc khảo mã số {don.MaDon} của bạn đã được nhân viên phòng đào tạo duyệt.

                Bạn sẽ nhận được kết quả chấm lại trong thời gian tới.

                Trân trọng,
                Phòng Đào Tạo
                """;

                try
                {
                    await _emailSen.SendEmailAsync(don.EmailSinhVien, subject, message);
                    don.DaGuiEmail = true;
                    _repository.Update(don); // đảm bảo repository có Update()
                    await _repository.SaveAsync(); // đảm bảo có SaveAsync()
                }
                catch (Exception ex)
                {
                    // Ghi log hoặc xử lý lỗi gửi email nếu cần
                    Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                }

            }
            return RedirectToAction("DanhSachChoDuyet");
        }

        // Upload bài thi
        public async Task<IActionResult> UploadBaiThi(int maDon)
        {
            var vm = await _repository.GetUploadViewModelAsync(maDon);
            if (vm == null) return NotFound();

            ViewBag.GiangViens = new SelectList(await _repository.GetGiangVienListAsync(), "MaGiangVien", "HoTen");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadBaiThi(UploadBaiThiViewModel model, List<IFormFile> files)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
            {
                ViewBag.GiangViens = new SelectList(await _repository.GetGiangVienListAsync(), "MaGiangVien", "HoTen", model.MaGiangVien);
                return View(model);
            }

            await _repository.LuuUploadAsync(model, files, user?.MaNhanVienPhongDaoTao);
            return RedirectToAction("DanhSachChoDuyet");
        }

        public async Task<IActionResult> ChiTietUpload(int maDon)
        {
            var vm = await _repository.GetChiTietUploadAsync(maDon);
            if (vm == null) return NotFound();
            return View(vm);
        }

        public async Task<IActionResult> EditUpload(int maDon)
        {
            var vm = await _repository.GetEditUploadAsync(maDon);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUpload(EditUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DanhSachAnh = (await _repository.GetChiTietUploadAsync(model.MaDon))?.DanhSachAnh ?? new();
                model.DanhSachGiangVien = new SelectList(await _repository.GetGiangVienListAsync(), "MaGiangVien", "HoTen", model.MaGiangVien);
                return View(model);
            }

            await _repository.LuuChinhSuaUploadAsync(model);
            return RedirectToAction("ChiTietUpload", new { maDon = model.MaDon });
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
