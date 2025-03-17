using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public class EFNamHocRepository : INamHocRepository {
        private readonly ApplicationDbContext _context;

        public EFNamHocRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NamHoc>> GetAllAsync()
        {
            return await _context.NamHocs.ToListAsync();
        }

        public async Task<NamHoc?> GetByIdAsync(int maNamHoc)
        {
            return await _context.NamHocs.FindAsync(maNamHoc);
        }

        public async Task AddAsync(NamHoc namHoc)
        {
            _context.NamHocs.Add(namHoc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NamHoc namHoc)
        {
            var existingNamHoc = await _context.NamHocs.FindAsync(namHoc.MaNamHoc);
            if (existingNamHoc != null)
            {
                _context.Entry(existingNamHoc).CurrentValues.SetValues(namHoc);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int maNamHoc)
        {
            var namHoc = await _context.NamHocs.FindAsync(maNamHoc);
            if (namHoc != null)
            {
                _context.NamHocs.Remove(namHoc);
                await _context.SaveChangesAsync();
            }
        }
    }
}
