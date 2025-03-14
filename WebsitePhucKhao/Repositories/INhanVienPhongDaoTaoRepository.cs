using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface INhanVienPhongDaoTaoRepository {
        Task<IEnumerable<NhanVienPhongDaoTao>> GetAllAsync();
        Task<NhanVienPhongDaoTao?> GetByIdAsync(int maNhanVien);
        Task AddAsync(NhanVienPhongDaoTao nhanVien);
        Task UpdateAsync(NhanVienPhongDaoTao nhanVien);
        Task DeleteAsync(int maNhanVien);
    }
}
