using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IBangDiemRepository {
        Task<IEnumerable<BangDiem>> GetAllAsync();
        Task<BangDiem?> GetByIdAsync(int id);
        Task AddAsync(BangDiem bangDiem);
        Task UpdateAsync(BangDiem bangDiem);
        Task DeleteAsync(int id);
        Task<IEnumerable<BangDiem>> GetBySinhVienIdAsync(int maSinhVien);
    }
}
