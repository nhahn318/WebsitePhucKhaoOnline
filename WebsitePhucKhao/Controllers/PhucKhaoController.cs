using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebsitePhucKhao.Repositories;


namespace WebsitePhucKhao.Controllers {
    public class PhucKhaoController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomEmailSender _emailSender;

        

        public PhucKhaoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ICustomEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
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
                 .Include(d => d.NamHoc)
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



        //======================================================SINHVIEN===============================================


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
                    EmailSinhVien = sinhVien.Email,
                    DaGuiEmail = false,
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
        //======================================================DUYET DON===============================================

        // Hiển thị danh sách đơn cần duyệt cho nhân viên
        public IActionResult DanhSachChoDuyet()
        {
            var danhSach = _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Where(d => d.TrangThai == "Chờ xác nhận" || d.TrangThai == "Đã duyệt")
                .ToList();

            return View(danhSach);
        }

        // Duyệt đơn
        public async Task<IActionResult> Duyet(int id)
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

        //======================================================UPLOAD===============================================


        public async Task<IActionResult> UploadBaiThi(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null || don.TrangThai != "Đã duyệt")
                return NotFound();

            ViewBag.GiangViens = new SelectList(await _context.GiangViens.ToListAsync(), "MaGiangVien", "HoTen");

            return View(new UploadBaiThiViewModel { MaDon = maDon });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadBaiThi(UploadBaiThiViewModel model, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GiangViens = new SelectList(_context.GiangViens, "MaGiangVien", "HoTen", model.MaGiangVien);
                return View(model);
            }

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // Lưu từng ảnh vào thư mục và DB
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _context.HinhAnhBaiThis.Add(new HinhAnhBaiThi
                    {
                        MaDon = model.MaDon,
                        DuongDanFile = "/uploads/" + fileName,
                        NgayTaiLen = DateTime.Now
                    });
                }
            }

            var don = await _context.DonPhucKhaos.FirstOrDefaultAsync(d => d.MaDon == model.MaDon);
            if (don == null)
            {
                return NotFound("Không tìm thấy đơn phúc khảo.");
            }

            // Lưu phân công giảng viên
            var chiTiet = new DonPhucKhaoChiTiet
            {
                MaDon = model.MaDon,
                MaGiangVien = model.MaGiangVien,
                MaMonHoc = don.MaMonHoc ?? 0,
                MaNhanVienDuyet = (await _userManager.GetUserAsync(User))?.MaNhanVienPhongDaoTao
            };
            _context.DonPhucKhaoChiTiets.Add(chiTiet);

            await _context.SaveChangesAsync();

            return RedirectToAction("DanhSachChoDuyet");
        }


        public async Task<IActionResult> ChiTietUpload(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null)
                return NotFound();

            var hinhAnh = await _context.HinhAnhBaiThis
                .Where(h => h.MaDon == maDon)
                .ToListAsync();

            var chiTiet = await _context.DonPhucKhaoChiTiets
                .Include(ct => ct.GiangVien)
                .FirstOrDefaultAsync(ct => ct.MaDon == maDon);

            var viewModel = new ChiTietUploadViewModel
            {
                Don = don,
                DanhSachAnh = hinhAnh,
                ChiTiet = chiTiet
            };

            return View(viewModel);
        }
        public async Task<IActionResult> EditUpload(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null)
                return NotFound();

            var hinhAnh = await _context.HinhAnhBaiThis
                .Where(h => h.MaDon == maDon)
                .ToListAsync();

            var chiTiet = await _context.DonPhucKhaoChiTiets
                .FirstOrDefaultAsync(ct => ct.MaDon == maDon);

            var viewModel = new EditUploadViewModel
            {
                MaDon = maDon,
                MaGiangVien = chiTiet?.MaGiangVien,
                DanhSachAnh = hinhAnh,
                DanhSachGiangVien = new SelectList(await _context.GiangViens.ToListAsync(), "MaGiangVien", "HoTen", chiTiet?.MaGiangVien)
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUpload(EditUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DanhSachAnh = await _context.HinhAnhBaiThis.Where(h => h.MaDon == model.MaDon).ToListAsync();
                model.DanhSachGiangVien = new SelectList(await _context.GiangViens.ToListAsync(), "MaGiangVien", "HoTen", model.MaGiangVien);
                return View(model);
            }

            // Xoá ảnh cũ nếu được chọn
            if (model.AnhCanXoa != null && model.AnhCanXoa.Count > 0)
            {
                var anhXoa = await _context.HinhAnhBaiThis
                    .Where(h => model.AnhCanXoa.Contains(h.MaHinhAnh))
                    .ToListAsync();

                foreach (var anh in anhXoa)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", anh.DuongDanFile.TrimStart('/'));
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);

                    _context.HinhAnhBaiThis.Remove(anh);
                }
            }

            // Lưu ảnh mới
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

            foreach (var file in model.AnhMoi ?? new List<IFormFile>())
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _context.HinhAnhBaiThis.Add(new HinhAnhBaiThi
                    {
                        MaDon = model.MaDon,
                        DuongDanFile = "/uploads/" + fileName,
                        NgayTaiLen = DateTime.Now
                    });
                }
            }

            // Cập nhật giảng viên nếu thay đổi
            var chiTiet = await _context.DonPhucKhaoChiTiets.FirstOrDefaultAsync(c => c.MaDon == model.MaDon);
            if (chiTiet != null && chiTiet.MaGiangVien != model.MaGiangVien)
            {
                chiTiet.MaGiangVien = model.MaGiangVien;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ChiTietUpload", new { maDon = model.MaDon });
        }


        [HttpGet]
        public async Task<IActionResult> Duyet(int id, bool chapNhan)
        {
            var don = await _context.DonPhucKhaos.FindAsync(id);
            if (don == null) return NotFound();

            // Cập nhật trạng thái đơn
            don.TrangThai = chapNhan ? "Đã duyệt" : "Từ chối";

            // Soạn nội dung email
            string ketQua = chapNhan ? "được DUYỆT" : "bị TỪ CHỐI";
            string noiDung = $@"
                                Chào bạn,

                                Đơn phúc khảo của bạn đã {ketQua}.
                                Trạng thái hiện tại: {don.TrangThai}

                                Trân trọng,
                                Phòng đào tạo
                                ";

            // Gửi email nếu chưa gửi
            if (!don.DaGuiEmail && !string.IsNullOrEmpty(don.EmailSinhVien))
            {
                await _emailSender.SendEmailAsync(don.EmailSinhVien, "Kết quả đơn phúc khảo", noiDung);
                don.DaGuiEmail = true;
            }

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            return RedirectToAction("DanhSachChoDuyet");
        }
    }

}
