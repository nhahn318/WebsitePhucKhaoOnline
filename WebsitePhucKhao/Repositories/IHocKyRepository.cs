using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IHocKyRepository {
        Task<IEnumerable<HocKy>> GetAllAsync();
        Task<HocKy?> GetByIdAsync(int maHocKy);
        Task AddAsync(HocKy hocKy);
        Task UpdateAsync(HocKy hocKy);
        Task DeleteAsync(int maHocKy);
    }
}
