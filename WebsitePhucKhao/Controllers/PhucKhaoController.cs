using Microsoft.AspNetCore.Mvc;

using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;

namespace WebsitePhucKhao.Controllers {
    public class PhucKhaoController : Controller {
        private readonly ApplicationDbContext _context;

        public PhucKhaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hiển thị form để sinh viên nhập thông tin
        public IActionResult Create()
        {
            return View();
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
                        Lop = model.Lop,
                        MatKhau = "default123" // Mật khẩu mặc định, có thể thay đổi sau
                    };
                    _context.SinhViens.Add(sinhVien);
                }
                // Thêm đơn phúc khảo mới
                var donPhucKhao = new DonPhucKhao
                {
                    MaSinhVien = model.MaSinhVien,
                    MonHoc = model.MonHoc,
                    DiemHienTai = model.DiemHienTai,
                    DiemMongMuon = model.DiemMongMuon,
                    HocKy = model.HocKy,
                    NamHoc = model.NamHoc,
                    NgayThi = model.NgayThi,
                    CaThi = model.CaThi,
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
