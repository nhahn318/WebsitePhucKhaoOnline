using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace WebsitePhucKhao.Controllers {
    public class PhucKhaoController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhucKhaoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<IActionResult> Details(int id)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.MonHoc)
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaDon == id);

            if (don == null)
                return NotFound();

            return View(don);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var don = await _context.DonPhucKhaos.FindAsync(id);
            if (don == null || don.TrangThai != "Chờ xác nhận")
                return NotFound();

            var viewModel = new SuaPhucKhaoViewModel
            {
                MaDon = don.MaDon,
                DiemHienTai = don.DiemHienTai,
                DiemMongMuon = don.DiemMongMuon,
                MaMonHoc = don.MaMonHoc,
                MaHocKy = don.HocKy,
                MaNamHoc = don.MaNamHoc,
                DiaDiemThi = don.DiaDiemThi,
                PhongThi = don.PhongThi,
                LyDo = don.LyDo,
                DanhSachMonHoc = new SelectList(await _context.MonHocs.ToListAsync(), "MaMonHoc", "TenMonHoc"),
                DanhSachHocKy = new SelectList(await _context.HocKys.ToListAsync(), "MaHocKy", "TenHocKy"),
                DanhSachNamHoc = new SelectList(await _context.NamHocs.ToListAsync(), "MaNamHoc", "TenNamHoc")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SuaPhucKhaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var don = await _context.DonPhucKhaos.FindAsync(model.MaDon);
                if (don == null || don.TrangThai != "Chờ xác nhận")
                    return NotFound();

                don.DiemHienTai = model.DiemHienTai;
                don.DiemMongMuon = model.DiemMongMuon;
                don.MaMonHoc = model.MaMonHoc;
                don.HocKy = model.MaHocKy ?? 0;
                don.MaNamHoc = model.MaNamHoc ?? 0;
                don.DiaDiemThi = model.DiaDiemThi;
                don.PhongThi = model.PhongThi;
                don.LyDo = model.LyDo;

                await _context.SaveChangesAsync();
                return RedirectToAction("DanhSachDonPhucKhao");
            }

            model.DanhSachMonHoc = new SelectList(await _context.MonHocs.ToListAsync(), "MaMonHoc", "TenMonHoc");
            model.DanhSachHocKy = new SelectList(await _context.HocKys.ToListAsync(), "MaHocKy", "TenHocKy");
            model.DanhSachNamHoc = new SelectList(await _context.NamHocs.ToListAsync(), "MaNamHoc", "TenNamHoc");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.MonHoc)
                .Include(d => d.SinhVien)    
                .Include(d => d.NamHoc)
                .FirstOrDefaultAsync(d => d.MaDon == id);

            if (don == null || don.TrangThai != "Chờ xác nhận")
                return NotFound();

            return View(don); // Hiển thị xác nhận xóa
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int maDon)
        {
            var don = await _context.DonPhucKhaos.FindAsync(maDon);
            if (don != null && don.TrangThai == "Chờ xác nhận")
            {
                _context.DonPhucKhaos.Remove(don);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("DanhSachDonPhucKhao");
        }



        //======================================================GIANGVIEN===============================================


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
            var namHocs = await _context.NamHocs.ToListAsync();

            var model = new YeuCauPhucKhaoViewModel
            {
                MaSinhVien = sinhVien.MaSinhVien,
                HoTen = sinhVien.HoTen,
                Email = sinhVien.Email,
                SoDienThoai = sinhVien.SoDienThoai,
                Lop = sinhVien.Lop?.TenLop ?? "Không có lớp",
                DanhSachMonHoc = new SelectList(monHocs, "MaMonHoc", "TenMonHoc"),
                DanhSachHocKy = new SelectList(hocKys, "MaHocKy", "TenHocKy"),
                DanhSachNamHoc = new SelectList(namHocs, "MaNamHoc", "TenNamHoc")
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
                var namHoc = await _context.NamHocs.FindAsync(model.MaNamHoc);

                var donPhucKhao = new DonPhucKhao
                {
                    MaSinhVien = model.MaSinhVien,
                    DiemHienTai = model.DiemHienTai,
                    DiemMongMuon = model.DiemMongMuon,
                    MaMonHoc = model.MaMonHoc,
                    MaLichThi = null,
                    MaGiangVien = model.MaGiangVien,
                    HocKy = model.MaHocKy ?? 0,
                    MaNamHoc = model.MaNamHoc ?? 0,
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
       
        //======================================================NHANVIEN===============================================
        
        // Hiển thị danh sách đơn cần duyệt cho nhân viên
        public IActionResult DanhSachChoDuyet()
        {
            var danhSach = _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Where(d => d.TrangThai == "Chờ xác nhận")
                .ToList();

            return View(danhSach);
        }

        // Duyệt đơn
        public async Task<IActionResult> Duyet(long id)
        {
            var don = await _context.DonPhucKhaos.FindAsync(id);
            if (don == null) return NotFound();

            var user = await _userManager.GetUserAsync(User); // lấy user hiện tại
            var maNV = user?.MaNhanVienPhongDaoTao;

            if (maNV == null)
                return Unauthorized(); // Không xác định được mã nhân viên

            don.MaNhanVienPhongDaoTao = maNV.Value;
            don.TrangThai = "Đã duyệt";
            await _context.SaveChangesAsync();

            return RedirectToAction("DanhSachChoDuyet");
        }
    }

}
