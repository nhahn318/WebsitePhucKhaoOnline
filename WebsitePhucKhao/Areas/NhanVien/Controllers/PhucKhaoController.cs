using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;
using WebsitePhucKhao.Repositories;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebsitePhucKhao.Areas.NhanVien.Controllers
{
    [Area("NhanVien")]
    [Authorize(Roles = "NhanVien")]
    public class PhucKhaoController : Controller
    {
        private readonly IPhucKhaoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ICustomEmailSender _emailSen;

        public PhucKhaoController(IPhucKhaoRepository repository, UserManager<ApplicationUser> userManager, ApplicationDbContext context, ICustomEmailSender emailSen)
        {
            _repository = repository;
            _userManager = userManager;
            _context = context;
            _emailSen = emailSen;
        }

        public async Task<IActionResult> DanhSachDon()
        {
            var ds = await _repository.GetDanhSachDonAsync();
            return View(ds);
        }
        public async Task<IActionResult> DanhSachPhucKhaoDaCham()
        {
            var ds = await _repository.GetDonDaChamAsync();
            return View("DanhSachPhucKhaoDaCham", ds);
        }

        public async Task<IActionResult> DanhSachPhucKhaoChoDuyet()
        {
            var ds = await _repository.GetDonChoXacNhanAsync();
            return View("DanhSachPhucKhaoChoDuyet", ds);
        }

        public async Task<IActionResult> DanhSachPhucKhaoDaDuyet()
        {
            var ds = await _repository.GetDonDaDuyetAsync();
            return View("DanhSachPhucKhaoDaDuyet", ds);
        }

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
            return RedirectToAction("DanhSachDon");
        }

        // Upload bài thi - Khi bấm nút LƯU sẽ upload bài thi và gửi mail cho giảng viên
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

            //Lấy thông tin giảng viên(để gửi email)
            var giangVien = await _context.GiangViens.FindAsync(model.MaGiangVien);
            var don = await _context.DonPhucKhaos.FindAsync(model.MaDon);

            if (giangVien != null && !string.IsNullOrEmpty(giangVien.Email))
            {
                string subject = "Thông báo chấm phúc khảo";
                string message = $@"
                Xin chào thầy/cô {giangVien.HoTen},

                Thầy/cô được phân công chấm đơn phúc khảo của sinh viên.

                Vui lòng đăng nhập vào hệ thống để xem và chấm lại bài thi.

                Trân trọng,
                Phòng đào tạo";

                // Bước 3: Gửi email
                await _emailSen.SendEmailAsync(giangVien.Email, subject, message);
            }
                return RedirectToAction("DanhSachDon");
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

     /*   public async Task<IActionResult> DanhSachPhucKhaoSauKhiCham()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var sinhVien = await _repository.GetSinhVienByEmailAsync(email);
            if (sinhVien == null) return NotFound();

            var danhSach = await _repository.GetDanhSachPhucKhaoViewModelAsync(sinhVien.MaSinhVien);

            var daCham = danhSach.Where(d => d.TrangThai == "Đã chấm").ToList();

            return View(daCham);
        }*/

        [HttpGet]
        public async Task<IActionResult> GetDiemHienTai(int maMonHoc, int maHocKy, int maNamHoc, long maSinhVien)
        {
            var diem = await _context.BangDiems
                .Where(b => b.MaMonHoc == maMonHoc
                         && b.MaHocKy == maHocKy
                         && b.MaNamHoc == maNamHoc
                         && b.MaSinhVien == maSinhVien)
                .Select(b => b.DiemCuoiKy)
                .FirstOrDefaultAsync();

            return Json(diem);
        }

        [HttpGet]
        public async Task<IActionResult> TimKiemDonChoDuyet(string keyword)
        {
            var ds = await _repository.GetDonChoXacNhanAsync(); 
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                ds = ds.Where(d =>
                    d.SinhVien != null &&
                    (d.SinhVien.MaSinhVien.ToString().Contains(keyword) ||
                     d.SinhVien.HoTen.ToLower().Contains(keyword))
                ).ToList();
            }

            var ketQua = ds.Select((d, i) => new
            {
                stt = i + 1,
                maDon = d.MaDon,
                tenSinhVien = d.SinhVien?.HoTen,
                tenMonHoc = d.MonHoc?.TenMonHoc,
                ngayGui = d.NgayGui.ToString("dd/MM/yyyy"),
                trangThai = d.TrangThai.ToFriendlyString()
            });

            return Json(ketQua);
        }

        [HttpGet]
        public async Task<IActionResult> TimKiemDonDaDuyet(string keyword)
        {
            var ds = await _repository.GetDonDaDuyetAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                ds = ds.Where(d =>
                    d.SinhVien != null &&
                    (d.SinhVien.MaSinhVien.ToString().Contains(keyword) ||
                     d.SinhVien.HoTen.ToLower().Contains(keyword))
                ).ToList();
            }

            var ketQua = ds.Select((d, i) => new
            {
                stt = i + 1,
                maDon = d.MaDon,
                tenSinhVien = d.SinhVien?.HoTen,
                tenMonHoc = d.MonHoc?.TenMonHoc,
                ngayGui = d.NgayGui.ToString("dd/MM/yyyy"),
                trangThai = d.TrangThai.ToFriendlyString()
            });

            return Json(ketQua);
        }

        [HttpGet]
        public async Task<IActionResult> TimKiemDonDaCham(string keyword)
        {
            var ds = await _repository.GetDonDaChamAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                ds = ds.Where(d =>
                    d.SinhVien != null &&
                    (d.SinhVien.MaSinhVien.ToString().Contains(keyword) ||
                     d.SinhVien.HoTen.ToLower().Contains(keyword))
                ).ToList();
            }

            var ketQua = ds.Select((d, i) => new
            {
                stt = i + 1,
                maDon = d.MaDon,
                tenSinhVien = d.SinhVien?.HoTen,
                tenMonHoc = d.MonHoc?.TenMonHoc,
                ngayGui = d.NgayGui.ToString("dd/MM/yyyy"),
                trangThai = d.TrangThai.ToFriendlyString()
            });

            return Json(ketQua);
        }
        [HttpGet]
        public async Task<IActionResult> TimKiemTatCaDon(string keyword)
        {
            var ds = await _repository.GetDanhSachDonAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                ds = ds.Where(d => d.SinhVien != null &&
                    (
                        d.SinhVien.MaSinhVien.ToString().Contains(keyword) ||
                        (!string.IsNullOrEmpty(d.SinhVien.HoTen) && d.SinhVien.HoTen.ToLower().Contains(keyword))
                    )).ToList();
            }

            var ketQua = ds.Select((d, i) => new
            {
                stt = i + 1,
                maDon = d.MaDon,
                tenSinhVien = d.SinhVien?.HoTen,
                tenMonHoc = d.MonHoc?.TenMonHoc,
                ngayGui = d.NgayGui.ToString("dd/MM/yyyy"),
                trangThai = d.TrangThai.ToFriendlyString()
            });

            return Json(ketQua);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new YeuCauPhucKhaoViewModel
            {
                DanhSachMonHoc = new SelectList(await _repository.GetMonHocListAsync(), "MaMonHoc", "TenMonHoc"),
                DanhSachHocKy = new SelectList(await _repository.GetHocKyListAsync(), "MaHocKy", "TenHocKy"),
                DanhSachNamHoc = new SelectList(await _repository.GetNamHocListAsync(), "MaNamHoc", "TenNamHoc")
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YeuCauPhucKhaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DanhSachMonHoc = new SelectList(await _repository.GetMonHocListAsync(), "MaMonHoc", "TenMonHoc");
                model.DanhSachHocKy = new SelectList(await _repository.GetHocKyListAsync(), "MaHocKy", "TenHocKy");
                model.DanhSachNamHoc = new SelectList(await _repository.GetNamHocListAsync(), "MaNamHoc", "TenNamHoc");
                return View(model);
            }

            var sinhVien = await _repository.GetSinhVienByMaAsync(model.MaSinhVien);
            if (sinhVien == null)
            {
                ModelState.AddModelError("MaSinhVien", "Không tìm thấy sinh viên.");
                return View(model);
            }

            // Kiểm tra đã có đơn cho môn học này chưa
            var daTonTai = await _repository.CheckDonPhucKhaoTonTaiAsync(
                model.MaSinhVien,
                model.MaMonHoc ?? 0,
                model.MaHocKy ?? 0,
                model.MaNamHoc ?? 0
            );

            if (daTonTai)
            {
                ModelState.AddModelError(string.Empty, "Sinh viên này đã gửi đơn phúc khảo cho môn học này rồi.");
                model.DanhSachMonHoc = new SelectList(await _repository.GetMonHocListAsync(), "MaMonHoc", "TenMonHoc");
                model.DanhSachHocKy = new SelectList(await _repository.GetHocKyListAsync(), "MaHocKy", "TenHocKy");
                model.DanhSachNamHoc = new SelectList(await _repository.GetNamHocListAsync(), "MaNamHoc", "TenNamHoc");
                return View(model);
            }

            // Gán lại thông tin từ hệ thống để đảm bảo độ chính xác
            model.HoTen = sinhVien.HoTen;
            model.Email = sinhVien.Email;
            model.SoDienThoai = sinhVien.SoDienThoai;
            model.Lop = sinhVien.Lop?.TenLop ?? "Không rõ";

            await _repository.TaoDonPhucKhaoAsync(model);

            TempData["Message"] = "Tạo đơn phúc khảo thành công!";
            return RedirectToAction("DanhSachDon");
        }


        [HttpGet]
        public async Task<IActionResult> GetSinhVienInfo(long maSinhVien)
        {
            var sv = await _repository.GetSinhVienByMaAsync(maSinhVien);
            if (sv == null) return NotFound();

            return Json(new
            {
                hoTen = sv.HoTen,
                email = sv.Email,
                soDienThoai = sv.SoDienThoai,
                lop = sv.Lop?.TenLop
            });
        }
    }
}

