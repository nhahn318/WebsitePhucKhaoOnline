using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public class EFSinhVienRepository : ISinhVienRepository {
        private readonly ApplicationDbContext _context;

        public EFSinhVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SinhVien>> GetAllAsync()
        {
            return await _context.SinhViens
                .Include(sv => sv.DonPhucKhaos) // Lấy thông tin các đơn phúc khảo (nếu muốn)
                .ToListAsync();
        }

        public async Task<SinhVien?> GetByIdAsync(long maSinhVien)
        {
            return await _context.SinhViens
                .Include(s => s.Khoa)
                .Include(s => s.ChuyenNganh)
                .Include(s => s.Lop)
                .FirstOrDefaultAsync(s => s.MaSinhVien == maSinhVien);
        }


        public async Task AddAsync(SinhVien sinhVien)
        {
            _context.SinhViens.Add(sinhVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SinhVien sinhVien)
        {
            var existingSinhVien = await _context.SinhViens.FindAsync(sinhVien.MaSinhVien);
            if (existingSinhVien != null)
            {
                _context.Entry(existingSinhVien).CurrentValues.SetValues(sinhVien);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(long maSinhVien)
        {
            var sinhVien = await _context.SinhViens.FindAsync(maSinhVien);
            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<SinhVien>> GetAllWithDetailsAsync()
        {
            return await _context.SinhViens
                .Include(sv => sv.Khoa) 
                .Include(sv => sv.ChuyenNganh) 
                .Include(sv => sv.Lop)
                .ToListAsync();
        }

        public async Task<SinhVien?> GetByEmailAsync(string email)
        {
            return await _context.SinhViens
                .Include(sv => sv.Khoa) 
                .Include(sv => sv.ChuyenNganh) 
                .FirstOrDefaultAsync(sv => sv.Email == email);
        }

    }
}