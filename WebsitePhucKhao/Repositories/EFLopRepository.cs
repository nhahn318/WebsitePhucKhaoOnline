using WebsitePhucKhao.Models;
using Microsoft.EntityFrameworkCore;

namespace WebsitePhucKhao.Repositories {
    public class EFLopRepository : ILopRepository {
        private readonly ApplicationDbContext _context;

        public EFLopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lop>> GetAllAsync()
        {
            return await _context.Lops
                .Include(l => l.Khoa) // Include Khoa nếu cần
                .ToListAsync();
        }

        public async Task<Lop?> GetByIdAsync(int maLop)
        {
            return await _context.Lops
                .Include(l => l.Khoa)
                .FirstOrDefaultAsync(l => l.MaLop == maLop);
        }

        public async Task AddAsync(Lop lop)
        {
            _context.Lops.Add(lop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lop lop)
        {
            var existingLop = await _context.Lops.FindAsync(lop.MaLop);
            if (existingLop != null)
            {
                _context.Entry(existingLop).CurrentValues.SetValues(lop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int maLop)
        {
            var lop = await _context.Lops.FindAsync(maLop);
            if (lop != null)
            {
                try
                {
                    _context.Lops.Remove(lop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("Không thể xóa lớp vì có sinh viên thuộc lớp này.");
                }
            }
        }


        public async Task<IEnumerable<Lop>> GetByKhoaIdAsync(int maKhoa)
        {
            return await _context.Lops
                .Where(l => l.MaKhoa == maKhoa)
                .ToListAsync();
        }
    }
}
