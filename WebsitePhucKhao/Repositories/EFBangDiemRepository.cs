using WebsitePhucKhao.Models;
using Microsoft.EntityFrameworkCore;

namespace WebsitePhucKhao.Repositories {
    public class EFBangDiemRepository : IBangDiemRepository {
        private readonly ApplicationDbContext _context;

        public EFBangDiemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BangDiem>> GetAllAsync()
        {
            return await _context.BangDiems
                .Include(bd => bd.SinhVien)
                .Include(bd => bd.MonHoc)
                .ToListAsync();
        }

        public async Task<BangDiem?> GetByIdAsync(int id)
        {
            return await _context.BangDiems
                .Include(bd => bd.SinhVien)
                .Include(bd => bd.MonHoc)
                .FirstOrDefaultAsync(bd => bd.Id == id);
        }

        public async Task<IEnumerable<BangDiem>> GetBySinhVienIdAsync(int maSinhVien)
        {
            return await _context.BangDiems
                .Where(bd => bd.MaSinhVien == maSinhVien)
                .Include(bd => bd.MonHoc)
                .ToListAsync();
        }

        public async Task AddAsync(BangDiem bangDiem)
        {
            _context.BangDiems.Add(bangDiem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BangDiem bangDiem)
        {
            var existingBangDiem = await _context.BangDiems.FindAsync(bangDiem.Id);
            if (existingBangDiem != null)
            {
                _context.Entry(existingBangDiem).CurrentValues.SetValues(bangDiem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var bangDiem = await _context.BangDiems.FindAsync(id);
            if (bangDiem != null)
            {
                _context.BangDiems.Remove(bangDiem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
