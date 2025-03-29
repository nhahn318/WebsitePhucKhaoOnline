using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IDonPhucKhaoChiTietRepository {
        Task<List<DonPhucKhaoChiTiet>> GetPhucKhaoByGiangVienAsync(long maGiangVien);
    }
}
