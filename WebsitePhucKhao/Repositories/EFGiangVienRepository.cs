using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebsitePhucKhao.Repositories {
    public class EFGiangVienRepository : IGiangVienRepository {
        private readonly ApplicationDbContext _context;

        public EFGiangVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GiangVien>> GetAllAsync()
        {
            return await _context.GiangViens
                .Include(g => g.Khoa) // Include navigation property Khoa
                .ToListAsync();
        }

        public async Task<GiangVien?> GetByIdAsync(long maGiangVien)
        {
            return await _context.GiangViens
                .Include(g => g.Khoa) // Đảm bảo load cả Khoa
                .FirstOrDefaultAsync(g => g.MaGiangVien == maGiangVien);
        }

        public async Task AddAsync(GiangVien giangVien)
        {
            _context.GiangViens.Add(giangVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GiangVien giangVien)
        {
            var existingGiangVien = await _context.GiangViens.FindAsync(giangVien.MaGiangVien);
            if (existingGiangVien != null)
            {
                _context.Entry(existingGiangVien).CurrentValues.SetValues(giangVien);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(long maGiangVien)
        {
            var giangVien = await _context.GiangViens.FindAsync(maGiangVien);
            if (giangVien != null)
            {
                _context.GiangViens.Remove(giangVien);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GiangVien>> GetAllWithDetailsAsync()
        {
            return await _context.GiangViens
                .Include(gv => gv.Khoa) // Nếu cần lấy thông tin Khoa của Giảng viên
                .ToListAsync();
        }

        public async Task<GiangVien> GetByEmailAsync(string email)
        {
            return await _context.GiangViens
                 .Include(g => g.Khoa)
                .FirstOrDefaultAsync(g => g.Email == email);
        }
     


    }
}
