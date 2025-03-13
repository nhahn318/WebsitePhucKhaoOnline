using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface ISinhVienRepository {
        Task<IEnumerable<SinhVien>> GetAllAsync();
        Task<SinhVien?> GetByIdAsync(int maSinhVien);
        Task AddAsync(SinhVien sinhVien);
        Task UpdateAsync(SinhVien sinhVien);
        Task DeleteAsync(int maSinhVien);
        public Task<IEnumerable<SinhVien>> GetAllWithDetailsAsync();

    }
}
