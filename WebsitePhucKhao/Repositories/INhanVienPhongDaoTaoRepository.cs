using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface INhanVienPhongDaoTaoRepository
    {
        Task<IEnumerable<NhanVienPhongDaoTao>> GetAllAsync();
        Task<NhanVienPhongDaoTao?> GetByIdAsync(long maNhanVien);
        Task AddAsync(NhanVienPhongDaoTao nhanVien);
        Task UpdateAsync(NhanVienPhongDaoTao nhanVien);
        Task DeleteAsync(long maNhanVien);
        Task<NhanVienPhongDaoTao?> GetByGmailAsync(string email);
    }
}
