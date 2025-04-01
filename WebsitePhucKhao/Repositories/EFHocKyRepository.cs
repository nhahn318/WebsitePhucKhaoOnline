using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public class EFHocKyRepository : IHocKyRepository {
        private readonly ApplicationDbContext _context;

        public EFHocKyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HocKy>> GetAllAsync()
        {
            return await _context.HocKys
            .Include(h => h.NamHoc)    
            .ToListAsync();
        }

        public async Task<HocKy?> GetByIdAsync(int maHocKy)
        {
            return await _context.HocKys.FindAsync(maHocKy);
        }

        public async Task AddAsync(HocKy hocKy)
        {
            _context.HocKys.Add(hocKy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HocKy hocKy)
        {
            var existingHocKy = await _context.HocKys.FindAsync(hocKy.MaHocKy);
            if (existingHocKy != null)
            {
                _context.Entry(existingHocKy).CurrentValues.SetValues(hocKy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int maHocKy)
        {
            var hocKy = await _context.HocKys.FindAsync(maHocKy);
            if (hocKy != null)
            {
                _context.HocKys.Remove(hocKy);
                await _context.SaveChangesAsync();
            }
        }
    }
}
