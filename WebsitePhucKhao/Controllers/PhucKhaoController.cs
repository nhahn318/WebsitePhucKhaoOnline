using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Create()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                ViewBag.ErrorMessage = "Bạn chưa đăng nhập.";
                return View();
            }

            var sinhVien = _context.SinhViens
                .Include(sv => sv.Lop)
                .FirstOrDefault(sv => sv.Email == userEmail);

            if (sinhVien == null)
            {
                ViewBag.ErrorMessage = "Không tìm thấy thông tin sinh viên với email: " + userEmail;
                return View();
            }

            var model = new YeuCauPhucKhaoViewModel
            {
                MaSinhVien = sinhVien.MaSinhVien,
                HoTen = sinhVien.HoTen,
                Email = sinhVien.Email,
                SoDienThoai = sinhVien.SoDienThoai,
                Lop = sinhVien.Lop?.TenLop ?? "Không có lớp"
            };

            return View(model);
        }



        // POST: Nhận dữ liệu từ form và lưu vào Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(YeuCauPhucKhaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra sinh viên đã tồn tại chưa
                var sinhVien = _context.SinhViens.Find(model.MaSinhVien);

                if (sinhVien == null)
                {
                    // Thêm sinh viên mới nếu chưa có
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
                // Thêm đơn phúc khảo mới
                var donPhucKhao = new DonPhucKhao
                {
                    MaSinhVien = model.MaSinhVien,
                    DiemHienTai = model.DiemHienTai,
                    DiemMongMuon = model.DiemMongMuon,
                    HocKy = model.HocKy,
                    NamHoc = model.NamHoc,
                    NhomLop = model.NhomLop,
                    DiaDiemThi = model.DiaDiemThi,
                    PhongThi = model.PhongThi,
                    LyDo = model.LyDo,
                    TrangThai = "Chờ xác nhận",
                    NgayGui = DateTime.Now
                };

                _context.DonPhucKhaos.Add(donPhucKhao);
                _context.SaveChanges();

                return RedirectToAction("CreateSuccess");
            }

            // Nếu dữ liệu không hợp lệ, trả về form với lỗi hiển thị
            return View(model);
        }

        // GET: Hiển thị thông báo thành công
        public IActionResult CreateSuccess()
        {
            return View();
        }
    }
}
