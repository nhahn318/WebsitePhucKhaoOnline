using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public class EFMonHocRepository : IMonHocRepository {
        private readonly ApplicationDbContext _context;

        public EFMonHocRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MonHoc>> GetAllAsync()
        {
            return await _context.MonHocs
                .Include(mh => mh.HocKy)
                .Include(mh => mh.GiangVien)
                .ToListAsync();
        }

        public async Task<MonHoc?> GetByIdAsync(int maMonHoc)
        {
            return await _context.MonHocs
                .Include(mh => mh.HocKy)
                .Include(mh => mh.GiangVien)
                .FirstOrDefaultAsync(mh => mh.MaMonHoc == maMonHoc);
        }

        public async Task<IEnumerable<MonHoc>> GetByHocKyAsync(int maHocKy)
        {
            return await _context.MonHocs
                .Where(mh => mh.MaHocKy == maHocKy)
                .Include(mh => mh.GiangVien)
                .ToListAsync();
        }

        public async Task AddAsync(MonHoc monHoc)
        {
            _context.MonHocs.Add(monHoc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MonHoc monHoc)
        {
            var existingMonHoc = await _context.MonHocs.FindAsync(monHoc.MaMonHoc);
            if (existingMonHoc != null)
            {
                _context.Entry(existingMonHoc).CurrentValues.SetValues(monHoc);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int maMonHoc)
        {
            var monHoc = await _context.MonHocs.FindAsync(maMonHoc);
            if (monHoc != null)
            {
                _context.MonHocs.Remove(monHoc);
                await _context.SaveChangesAsync();
            }
        }
    }
}
