using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface INamHocRepository {
        Task<IEnumerable<NamHoc>> GetAllAsync();
        Task<NamHoc?> GetByIdAsync(int maNamHoc);
        Task AddAsync(NamHoc namHoc);
        Task UpdateAsync(NamHoc namHoc);
        Task DeleteAsync(int maNamHoc);
    }
}
