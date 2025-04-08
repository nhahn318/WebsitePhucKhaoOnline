using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Enums;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public class EFDonPhucKhaoChiTietRepository : IDonPhucKhaoChiTietRepository {
        private readonly ApplicationDbContext _context;

        public EFDonPhucKhaoChiTietRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DonPhucKhaoChiTiet>> GetPhucKhaoByGiangVienAsync(long maGiangVien)
        {
            var danhSach = await _context.DonPhucKhaoChiTiets
            .Include(d => d.GiangVien)
            .Include(d => d.SinhVien)
            .Include(d => d.DonPhucKhao)           
            .Include(d => d.NhanVienDuyet)
            .Include(d => d.MonHoc)
            .Where(d => d.MaGiangVien == maGiangVien)
            .ToListAsync();

            Console.WriteLine($"DEBUG: Tìm thấy {danhSach.Count} đơn cho giảng viên {maGiangVien}");
            foreach (var don in danhSach)
            {
                Console.WriteLine($"DEBUG: Đơn {don.MaDon} - Sinh viên ID: {don.MaSinhVien}, Tên: {don.SinhVien?.HoTen}");
            }
            return danhSach;
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

    }
}
