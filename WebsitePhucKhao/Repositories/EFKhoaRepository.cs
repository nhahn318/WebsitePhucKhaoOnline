using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public class EFKhoaRepository : IKhoaRepository {
        private readonly ApplicationDbContext _context;

        public EFKhoaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả các khoa
        public async Task<IEnumerable<Khoa>> GetAllAsync()
        {
            return await _context.Khoas
                .Include(k => k.ChuyenNganhs) // Lấy thông tin các chuyên ngành của khoa (nếu cần)
                .ToListAsync();
        }

        // Lấy thông tin một khoa theo ID
        public async Task<Khoa?> GetByIdAsync(int maKhoa)
        {
            return await _context.Khoas
                .Include(k => k.ChuyenNganhs) // Nếu cần thông tin liên quan
                .FirstOrDefaultAsync(k => k.MaKhoa == maKhoa);
        }

        // Thêm một khoa mới vào database
        public async Task AddAsync(Khoa khoa)
        {
            _context.Khoas.Add(khoa);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin khoa
        public async Task UpdateAsync(Khoa khoa)
        {
            var existingKhoa = await _context.Khoas.FindAsync(khoa.MaKhoa);
            if (existingKhoa != null)
            {
                _context.Entry(existingKhoa).CurrentValues.SetValues(khoa);
                await _context.SaveChangesAsync();
            }
        }

        // Xóa một khoa khỏi database
        public async Task DeleteAsync(int maKhoa)
        {
            var khoa = await _context.Khoas.FindAsync(maKhoa);
            if (khoa != null)
            {
                _context.Khoas.Remove(khoa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
