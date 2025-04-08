using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IGiangVienRepository {
        Task<IEnumerable<GiangVien>> GetAllAsync();
        Task<GiangVien?> GetByIdAsync(long maGiangVien);
        Task AddAsync(GiangVien giangVien);
        Task UpdateAsync(GiangVien giangVien);
        Task DeleteAsync(long maGiangVien);
        Task<IEnumerable<GiangVien>> GetAllWithDetailsAsync();
        Task<GiangVien> GetByEmailAsync(string email);

    }
}
