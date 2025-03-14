using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebsitePhucKhao.Repositories {
    public class EFNhanVienPhongDaoTaoRepository : INhanVienPhongDaoTaoRepository {
        private readonly ApplicationDbContext _context;

        public EFNhanVienPhongDaoTaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhanVienPhongDaoTao>> GetAllAsync()
        {
            return await _context.NhanVienPhongDaoTaos.ToListAsync();
        }

        public async Task<NhanVienPhongDaoTao?> GetByIdAsync(int maNhanVien)
        {
            return await _context.NhanVienPhongDaoTaos.FindAsync(maNhanVien);
        }

        public async Task AddAsync(NhanVienPhongDaoTao nhanVien)
        {
            _context.NhanVienPhongDaoTaos.Add(nhanVien);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhanVienPhongDaoTao nhanVien)
        {
            var existingNhanVien = await _context.NhanVienPhongDaoTaos.FindAsync(nhanVien.MaNhanVienPhongDaoTao);
            if (existingNhanVien != null)
            {
                _context.Entry(existingNhanVien).CurrentValues.SetValues(nhanVien);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int maNhanVien)
        {
            var nhanVien = await _context.NhanVienPhongDaoTaos.FindAsync(maNhanVien);
            if (nhanVien != null)
            {
                _context.NhanVienPhongDaoTaos.Remove(nhanVien);
                await _context.SaveChangesAsync();
            }
        }
    }
}
