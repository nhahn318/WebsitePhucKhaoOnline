using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IMonHocRepository {
        Task<IEnumerable<MonHoc>> GetAllAsync();
        Task<MonHoc?> GetByIdAsync(int maMonHoc);
        Task AddAsync(MonHoc monHoc);
        Task UpdateAsync(MonHoc monHoc);
        Task DeleteAsync(int maMonHoc);
        Task<IEnumerable<MonHoc>> GetByHocKyAsync(int maHocKy);
    }
}
