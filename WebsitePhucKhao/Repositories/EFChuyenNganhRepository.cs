using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public class EFChuyenNganhRepository : IChuyenNganhRepository {
        private readonly ApplicationDbContext _context;

        public EFChuyenNganhRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChuyenNganh>> GetAllAsync()
        {
            return await _context.ChuyenNganhs.Include(cn => cn.Khoa).ToListAsync();
        }

        public async Task<ChuyenNganh> GetByIdAsync(int id)
        {
            return await _context.ChuyenNganhs
                .Include(cn => cn.Khoa)
                .FirstOrDefaultAsync(cn => cn.MaChuyenNganh == id);
        }

        public async Task<IEnumerable<ChuyenNganh>> GetByKhoaIdAsync(int maKhoa)
        {
            return await _context.ChuyenNganhs
                .Where(cn => cn.MaKhoa == maKhoa)
                .ToListAsync();
        }


        public async Task AddAsync(ChuyenNganh chuyenNganh)
        {
            await _context.ChuyenNganhs.AddAsync(chuyenNganh);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChuyenNganh chuyenNganh)
        {
            _context.ChuyenNganhs.Update(chuyenNganh);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chuyenNganh = await _context.ChuyenNganhs.FindAsync(id);
            if (chuyenNganh != null)
            {
                _context.ChuyenNganhs.Remove(chuyenNganh);
                await _context.SaveChangesAsync();
            }
        }
    }
}
