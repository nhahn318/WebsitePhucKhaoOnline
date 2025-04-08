using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Enums;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;

namespace WebsitePhucKhao.Repositories {
    public class EFPhucKhaoRepository : IPhucKhaoRepository {
        private readonly ApplicationDbContext _context;
        public EFPhucKhaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SinhVien?> GetSinhVienByEmailAsync(string email)
        {
            return await _context.SinhViens
                .Include(sv => sv.Lop)
                .FirstOrDefaultAsync(sv => sv.Email == email);
        }

        public async Task<List<DonPhucKhao>> GetDanhSachPhucKhaoSinhVienAsync(int maSinhVien)
        {
            return await _context.DonPhucKhaos
                .Where(d => d.MaSinhVien == maSinhVien)
                .OrderByDescending(d => d.NgayGui)
                .ToListAsync();
        }

        public async Task<DonPhucKhao?> GetDonPhucKhaoAsync(int id)
        {
            return await _context.DonPhucKhaos

                .Include(d => d.MonHoc)
                .Include(d => d.SinhVien)
                .Include(d => d.NamHoc)
                .Include(d => d.HocKy)
                .FirstOrDefaultAsync(d => d.MaDon == id);
        }

        public async Task<List<DonPhucKhao>> GetDonChoXacNhanAsync()
        {
            return await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Where(d => d.TrangThai == TrangThaiPhucKhao.ChoXacNhan)
                .ToListAsync();
        }

        public async Task<List<DonPhucKhao>> GetDonDaDuyetAsync()
        {
            return await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Where(d => d.TrangThai == TrangThaiPhucKhao.DaDuyet)
                .ToListAsync();
        }

        public async Task<List<DonPhucKhao>> GetDonDaChamAsync()
        {
            return await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Where(d => d.TrangThai == TrangThaiPhucKhao.DaCham)
                .ToListAsync();
        }

        public async Task<List<DonPhucKhaoChiTiet>> GetDaChamByGiangVienAsync(long maGiangVien)
        {
            return await _context.DonPhucKhaoChiTiets
                .Include(ct => ct.DonPhucKhao)
                .ThenInclude(d => d.SinhVien)
                .Include(ct => ct.MonHoc)
                .Include(ct => ct.NhanVienDuyet)
                .Where(ct => ct.MaGiangVien == maGiangVien && ct.DonPhucKhao.TrangThai == TrangThaiPhucKhao.DaCham)
                .ToListAsync();
        }




        public async Task<SuaPhucKhaoViewModel?> GetSuaPhucKhaoViewModelAsync(int id)
        {
            var don = await _context.DonPhucKhaos.FindAsync(id);
            if (don == null || don.TrangThai != TrangThaiPhucKhao.ChoXacNhan) return null;

            return new SuaPhucKhaoViewModel
            {
                MaDon = don.MaDon,
                DiemHienTai = don.DiemHienTai,
                DiemMongMuon = don.DiemMongMuon,
                MaMonHoc = don.MaMonHoc,
                MaHocKy = don.MaHocKy,
                MaNamHoc = don.MaNamHoc,
                DiaDiemThi = don.DiaDiemThi,
                PhongThi = don.PhongThi,
                LyDo = don.LyDo,
                DanhSachMonHoc = new SelectList(await _context.MonHocs.ToListAsync(), "MaMonHoc", "TenMonHoc"),
                DanhSachHocKy = new SelectList(await _context.HocKys.ToListAsync(), "MaHocKy", "TenHocKy"),
                DanhSachNamHoc = new SelectList(await _context.NamHocs.ToListAsync(), "MaNamHoc", "TenNamHoc")
            };
        }

        public async Task<bool> CapNhatDonPhucKhaoAsync(SuaPhucKhaoViewModel model)
        {
            var don = await _context.DonPhucKhaos.FindAsync(model.MaDon);
            if (don == null || don.TrangThai != TrangThaiPhucKhao.ChoXacNhan) return false;

            don.DiemHienTai = model.DiemHienTai;
            don.DiemMongMuon = model.DiemMongMuon;
            don.MaMonHoc = model.MaMonHoc;
            don.MaHocKy = model.MaHocKy ?? 0;
            don.MaNamHoc = model.MaNamHoc ?? 0;
            don.DiaDiemThi = model.DiaDiemThi;
            don.PhongThi = model.PhongThi;
            don.LyDo = model.LyDo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> XacNhanXoaDonAsync(int id)
        {
            var don = await GetDonPhucKhaoAsync(id);
            return don != null && don.TrangThai == TrangThaiPhucKhao.ChoXacNhan;
        }

        public async Task<bool> XoaDonAsync(int id)
        {
            var don = await _context.DonPhucKhaos.FindAsync(id);
            if (don == null || don.TrangThai != TrangThaiPhucKhao.ChoXacNhan) return false;

            _context.DonPhucKhaos.Remove(don);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<YeuCauPhucKhaoViewModel?> GetYeuCauPhucKhaoViewModelAsync(string email)
        {
            var sinhVien = await GetSinhVienByEmailAsync(email);
            if (sinhVien == null)
                return null;

            var today = DateTime.Now;

            // Chỉ lấy học kỳ đang trong thời gian phúc khảo
            var hocKy = await _context.HocKys
                .Where(hk => today >= hk.NgayBatDauPhucKhao && today <= hk.NgayKetThucPhucKhao)
                .OrderByDescending(hk => hk.NgayBatDauPhucKhao)
                .FirstOrDefaultAsync();

            if (hocKy == null)
                return null;

            // Lấy danh sách môn học đã thi trong học kỳ đó
            var monDaThi = await _context.BangDiems
                .Where(b => b.MaSinhVien == sinhVien.MaSinhVien
                            && b.MaHocKy == hocKy.MaHocKy
                            && b.MaNamHoc == hocKy.MaNamHoc
                            && b.DiemTongKet != null)
                .Select(b => b.MonHoc)
                .Distinct()
                .ToListAsync();

            var monHocDauTien = monDaThi.FirstOrDefault();
            LichThi? lichThiDauTien = null;

            if (monHocDauTien != null)
            {
                // Lấy lịch thi tương ứng với môn học đầu tiên (nếu có)
                lichThiDauTien = await _context.LichThis
                    .FirstOrDefaultAsync(l => l.MaMonHoc == monHocDauTien.MaMonHoc
                                           && l.MaHocKy == hocKy.MaHocKy
                                           && l.MaNamHoc == hocKy.MaNamHoc);
            }

            return new YeuCauPhucKhaoViewModel
            {
                MaSinhVien = sinhVien.MaSinhVien,
                HoTen = sinhVien.HoTen,
                Email = sinhVien.Email,
                SoDienThoai = sinhVien.SoDienThoai,
                Lop = sinhVien.Lop?.TenLop ?? "Không có lớp",
                DiemHienTai = 0,

                DanhSachMonHoc = new SelectList(monDaThi, "MaMonHoc", "TenMonHoc"),
                DanhSachHocKy = new SelectList(await _context.HocKys.ToListAsync(), "MaHocKy", "TenHocKy"),
                DanhSachNamHoc = new SelectList(await _context.NamHocs.ToListAsync(), "MaNamHoc", "TenNamHoc"),

                NgayBatDauPhucKhao = hocKy.NgayBatDauPhucKhao,
                NgayKetThucPhucKhao = hocKy.NgayKetThucPhucKhao,

                NgayThi = lichThiDauTien?.NgayThi,
                CaThi = lichThiDauTien?.CaThi,
                PhongThi = lichThiDauTien?.PhongThi,
                DiaDiemThi = lichThiDauTien?.DiaDiemThi
            };

        }

        public async Task<double?> GetDiemHienTaiAsync(long maSinhVien, int maMonHoc, int maHocKy, int maNamHoc)
        {
            var diem = await _context.BangDiems
                .Where(b => b.MaSinhVien == maSinhVien &&
                            b.MaMonHoc == maMonHoc &&
                            b.MaHocKy == maHocKy &&
                            b.MaNamHoc == maNamHoc)
                .Select(b => b.DiemTongKet)
                .FirstOrDefaultAsync();

            return diem;
        }


        public async Task<bool> TaoDonPhucKhaoAsync(YeuCauPhucKhaoViewModel model)
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

            var don = new DonPhucKhao
            {
                MaSinhVien = model.MaSinhVien,
                DiemHienTai = model.DiemHienTai,
                DiemMongMuon = model.DiemMongMuon,
                MaMonHoc = model.MaMonHoc,
                MaHocKy = model.MaHocKy ?? 0,
                MaNamHoc = model.MaNamHoc ?? 0,
                DiaDiemThi = model.DiaDiemThi,
                PhongThi = model.PhongThi,
                LyDo = model.LyDo,
                TrangThai = TrangThaiPhucKhao.ChoXacNhan,
                NgayGui = DateTime.Now,
                EmailSinhVien = sinhVien?.Email ?? ""
            };

            _context.DonPhucKhaos.Add(don);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DonPhucKhao>> GetDanhSachDonAsync()
        {
            return await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Where(d => d.TrangThai == TrangThaiPhucKhao.ChoXacNhan || d.TrangThai == TrangThaiPhucKhao.DaDuyet || d.TrangThai == TrangThaiPhucKhao.ChoCham || d.TrangThai == TrangThaiPhucKhao.ChoCham || d.TrangThai == TrangThaiPhucKhao.DaCham)
                .ToListAsync();
        }

        public async Task<bool> DuyetDonAsync(int id, long? maNV)
        {
            var don = await _context.DonPhucKhaos.FindAsync(id);
            if (don == null || maNV == null) return false;

            don.MaNhanVienPhongDaoTao = maNV;
            don.TrangThai = TrangThaiPhucKhao.DaDuyet;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UploadBaiThiViewModel?> GetUploadViewModelAsync(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null || don.TrangThai != TrangThaiPhucKhao.DaDuyet) return null;

            return new UploadBaiThiViewModel
            {
                MaDon = maDon
            };
        }

        public async Task<bool> LuuUploadAsync(UploadBaiThiViewModel model, List<IFormFile> files, long? maNV)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    _context.HinhAnhBaiThis.Add(new HinhAnhBaiThi
                    {
                        MaDon = model.MaDon,
                        DuongDanFile = "/uploads/" + fileName,
                        NgayTaiLen = DateTime.Now
                    });
                }
            }

            var don = await _context.DonPhucKhaos.FirstOrDefaultAsync(d => d.MaDon == model.MaDon);
            if (don == null) return false;

            _context.DonPhucKhaoChiTiets.Add(new DonPhucKhaoChiTiet
            {
                MaDon = model.MaDon,
                MaGiangVien = model.MaGiangVien,
                MaMonHoc = don.MaMonHoc ?? 0,
                MaNhanVienDuyet = maNV
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ChiTietUploadViewModel?> GetChiTietUploadAsync(int maDon)
        {
            var don = await GetDonPhucKhaoAsync(maDon);
            if (don == null) return null;

            var hinhAnh = await _context.HinhAnhBaiThis.Where(h => h.MaDon == maDon).ToListAsync();
            var chiTiet = await _context.DonPhucKhaoChiTiets.Include(ct => ct.GiangVien).FirstOrDefaultAsync(ct => ct.MaDon == maDon);

            return new ChiTietUploadViewModel
            {
                Don = don,
                DanhSachAnh = hinhAnh,
                ChiTiet = chiTiet
            };
        }

        public async Task<EditUploadViewModel?> GetEditUploadAsync(int maDon)
        {
            var chiTiet = await _context.DonPhucKhaoChiTiets.FirstOrDefaultAsync(ct => ct.MaDon == maDon);
            var hinhAnh = await _context.HinhAnhBaiThis.Where(h => h.MaDon == maDon).ToListAsync();

            return new EditUploadViewModel
            {
                MaDon = maDon,
                MaGiangVien = chiTiet?.MaGiangVien,
                DanhSachAnh = hinhAnh,
                DanhSachGiangVien = new SelectList(await _context.GiangViens.ToListAsync(), "MaGiangVien", "HoTen", chiTiet?.MaGiangVien)
            };
        }

        public async Task<bool> LuuChinhSuaUploadAsync(EditUploadViewModel model)
        {
            // Xoá ảnh được chọn
            if (model.AnhCanXoa != null)
            {
                var anhXoa = await _context.HinhAnhBaiThis.Where(h => model.AnhCanXoa.Contains(h.MaHinhAnh)).ToListAsync();
                foreach (var anh in anhXoa)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", anh.DuongDanFile.TrimStart('/'));
                    if (File.Exists(fullPath)) File.Delete(fullPath);
                    _context.HinhAnhBaiThis.Remove(anh);
                }
            }

            // Lưu ảnh mới
            if (model.AnhMoi != null)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

                foreach (var file in model.AnhMoi)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadFolder, fileName);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await file.CopyToAsync(stream);

                        _context.HinhAnhBaiThis.Add(new HinhAnhBaiThi
                        {
                            MaDon = model.MaDon,
                            DuongDanFile = "/uploads/" + fileName,
                            NgayTaiLen = DateTime.Now
                        });
                    }
                }
            }

            // Cập nhật giảng viên
            var chiTiet = await _context.DonPhucKhaoChiTiets.FirstOrDefaultAsync(c => c.MaDon == model.MaDon);
            if (chiTiet != null && chiTiet.MaGiangVien != model.MaGiangVien)
            {
                chiTiet.MaGiangVien = model.MaGiangVien;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MonHoc>> GetMonHocListAsync()
        {
            return await _context.MonHocs.ToListAsync();
        }
        public async Task<List<HocKy>> GetHocKyListAsync()
        {
            return await _context.HocKys.ToListAsync();
        }
        public async Task<List<NamHoc>> GetNamHocListAsync()
        {
            return await _context.NamHocs.ToListAsync();
        }
        public async Task<List<GiangVien>> GetGiangVienListAsync()
        {
            return await _context.GiangViens.ToListAsync();
        }
        public async Task<List<DonPhucKhao>> GetDanhSachPhucKhaoSinhVienAsync(long maSinhVien)
        {
            return await _context.DonPhucKhaos
                .Where(d => d.MaSinhVien == maSinhVien)
                .OrderByDescending(d => d.NgayGui)
                .ToListAsync();
        }
        public async Task<List<DonPhucKhaoSauKhiChamViewModel>> GetDanhSachPhucKhaoViewModelAsync(long maSinhVien)
        {
            var danhSach = await _context.DonPhucKhaos
                .Include(d => d.ChiTietPhucKhao)
                .Where(d => d.MaSinhVien == maSinhVien)
                .OrderByDescending(d => d.NgayGui)
                .ToListAsync();

            return danhSach
                .Select((d, index) => new DonPhucKhaoSauKhiChamViewModel
                {
                    STT = index + 1,
                    MaDon = d.MaDon,
                    NgayGui = d.NgayGui,
                    TrangThai = d.TrangThai.ToFriendlyString(), // ✅ chuyển enum sang chuỗi
                    DiemHienTai = d.DiemHienTai,
                    DiemSauPhucKhao = d.ChiTietPhucKhao?.DiemSauPhucKhao,
                    NhanXet = d.ChiTietPhucKhao?.NhanXet,
                    BaiGiaiTayUrl = d.ChiTietPhucKhao?.BaiGiaiTayUrl
                })
                .ToList();
        }

      


        public void Update(DonPhucKhao don)
        {
            _context.Update(don);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<SinhVien?> GetSinhVienByMaAsync(long maSinhVien)
        {
            return await _context.SinhViens
                .Include(sv => sv.Lop)
                .FirstOrDefaultAsync(sv => sv.MaSinhVien == maSinhVien);
        }

    }
}
