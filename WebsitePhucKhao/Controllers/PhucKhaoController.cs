using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;

namespace WebsitePhucKhao.Controllers {
    public class PhucKhaoController : Controller {
        private readonly ApplicationDbContext _context;

        public PhucKhaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DanhSachDonPhucKhao()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var sinhVien = await _context.SinhViens.FirstOrDefaultAsync(sv => sv.Email == userEmail);
            if (sinhVien == null)
            {
                return NotFound("Không tìm thấy sinh viên.");
            }

            var danhSachDb = await _context.DonPhucKhaos
                .Where(d => d.MaSinhVien == sinhVien.MaSinhVien)
                .OrderByDescending(d => d.NgayGui)
                .ToListAsync();

            var danhSach = danhSachDb
                .Select((d, index) => new
                {
                    STT = index + 1,
                    d.NgayGui,
                    d.TrangThai,
                    d.MaDon
                })
                .ToList();

            return View(danhSach);
        }

        // GET: Hiển thị form tạo đơn phúc khảo
        public async Task<IActionResult> Create()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                ViewBag.ErrorMessage = "Bạn chưa đăng nhập.";
                return View();
            }

            var sinhVien = await _context.SinhViens
                .Include(sv => sv.Lop)
                .FirstOrDefaultAsync(sv => sv.Email == userEmail);

            if (sinhVien == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy sinh viên.";
                return View();
            }

            // Lấy học kỳ hiện tại của sinh viên
            var hocKy = await _context.HocKys.OrderByDescending(hk => hk.NgayBatDauPhucKhao).FirstOrDefaultAsync();
            if (hocKy == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy học kỳ có thời gian phúc khảo.";
                return View("OutsidePhucKhao");
            }

            var today = DateTime.Now;

            // Kiểm tra xem có nằm trong thời gian phúc khảo không
            if (today < hocKy.NgayBatDauPhucKhao || today > hocKy.NgayKetThucPhucKhao)
            {
                ViewBag.ErrorMessage = "Hiện tại không phải là thời gian phúc khảo.";
                return View("OutsidePhucKhao");
            }


            var monHocs = await _context.MonHocs.ToListAsync();
            var hocKys = await _context.HocKys.ToListAsync();

            var model = new YeuCauPhucKhaoViewModel
            {
                MaSinhVien = sinhVien.MaSinhVien,
                HoTen = sinhVien.HoTen,
                Email = sinhVien.Email,
                SoDienThoai = sinhVien.SoDienThoai,
                Lop = sinhVien.Lop?.TenLop ?? "Không có lớp",
                DanhSachMonHoc = new SelectList(monHocs, "MaMonHoc", "TenMonHoc"),
                DanhSachHocKy = new SelectList(hocKys, "MaHocKy", "TenHocKy")
            };

            return View(model);
        }

        // POST: Nhận dữ liệu từ form và lưu vào Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YeuCauPhucKhaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sinhVien = await _context.SinhViens.FindAsync(model.MaSinhVien);

                if (sinhVien == null)
                {
                    sinhVien = new SinhVien
                    {
                        MaSinhVien = model.MaSinhVien,
                        HoTen = model.HoTen,
                        Email = model.Email,
                        SoDienThoai = model.SoDienThoai,
                        MatKhau = "default123"
                    };
                    _context.SinhViens.Add(sinhVien);
                }

                var donPhucKhao = new DonPhucKhao
                {
                    MaSinhVien = model.MaSinhVien,
                    DiemHienTai = model.DiemHienTai,
                    DiemMongMuon = model.DiemMongMuon,
                    MaMonHoc = model.MaMonHoc,
                    MaLichThi = null,
                    MaGiangVien = model.MaGiangVien,
                    HocKy = model.MaHocKy ?? 0,
                    NamHoc = model.NamHoc,
                    DiaDiemThi = model.DiaDiemThi,
                    PhongThi = model.PhongThi,
                    LyDo = model.LyDo,
                    TrangThai = "Chờ xác nhận",
                    NgayGui = DateTime.Now
                };

                _context.DonPhucKhaos.Add(donPhucKhao);
                await _context.SaveChangesAsync();

                return RedirectToAction("CreateSuccess");
            }

            // Load lại SelectList nếu lỗi
            model.DanhSachMonHoc = new SelectList(await _context.MonHocs.ToListAsync(), "MaMonHoc", "TenMonHoc");
            model.DanhSachHocKy = new SelectList(await _context.HocKys.ToListAsync(), "MaHocKy", "TenHocKy");

            return View(model);
        }

        public IActionResult CreateSuccess()
        {
            return View();
        }
    }

}
