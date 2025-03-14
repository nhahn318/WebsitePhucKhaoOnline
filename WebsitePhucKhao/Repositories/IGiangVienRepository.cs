using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IGiangVienRepository {
        Task<IEnumerable<GiangVien>> GetAllAsync();
        Task<GiangVien?> GetByIdAsync(int maGiangVien);
        Task AddAsync(GiangVien giangVien);
        Task UpdateAsync(GiangVien giangVien);
        Task DeleteAsync(int maGiangVien);
        Task<IEnumerable<GiangVien>> GetAllWithDetailsAsync();
    }
}
