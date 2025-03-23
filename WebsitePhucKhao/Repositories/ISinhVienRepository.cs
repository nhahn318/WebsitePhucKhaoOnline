using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface ISinhVienRepository {
        Task<IEnumerable<SinhVien>> GetAllAsync();
        Task<SinhVien?> GetByIdAsync(long maSinhVien);
        Task AddAsync(SinhVien sinhVien);
        Task UpdateAsync(SinhVien sinhVien);
        Task DeleteAsync(long maSinhVien);
        public Task<IEnumerable<SinhVien>> GetAllWithDetailsAsync();
        Task<SinhVien?> GetByEmailAsync(string email);
    }
}
